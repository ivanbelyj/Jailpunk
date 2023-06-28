using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// A component of the lifecycle of a living being
///</summary>
public class EntityLifeCycle : NetworkBehaviour
{
    #region Parameters and effects
    [Header("Parameters")]
    [SerializeField]
    private LifeCycleParameter health;
    [SerializeField]
    private LifeCycleParameter endurance;
    [SerializeField]
    private LifeCycleParameter satiety;
    [SerializeField]
    private LifeCycleParameter bleed;
    [SerializeField]
    private LifeCycleParameter radiation;

    [Header("Constant effects")]
    [SerializeField]
    private LifeCycleEffect regeneration;
    [SerializeField]
    private LifeCycleEffect enduranceRecovery;
    [SerializeField]
    private LifeCycleEffect hunger;
    [SerializeField]
    private LifeCycleEffect radiationExcretion;

    [Header("Temporary effects")]
    [SerializeField]
    private LifeCycleEffect runEnduranceDecrease;
    [SerializeField]
    private LifeCycleEffect bleedEffect;
    [SerializeField]
    private LifeCycleEffect radiationPoisoning;
    #endregion

    private readonly SyncHashSet<LifeCycleEffect> syncEffects =
        new SyncHashSet<LifeCycleEffect>();

    private HashSet<LifeCycleEffect> effects;

    ///<summary>
    /// All uncompleted effects applied to change the parameters of the life cycle.
    /// Completed effects are removed after regular traversal 
    ///</summary>
    public HashSet<LifeCycleEffect> Effects => effects;

    ///<summary>
    /// All lifecycle parameters collected for convenient traversal.
    /// Dynamic addition / removal of parameters is not assumed
    ///</summary>
    public IReadOnlyDictionary<LifeCycleParameterEnum, LifeCycleParameter> Parameters { get; private set; }

    public bool IsAlive { get; private set; }
    public event UnityAction OnDeath;

    private void Awake() {
        syncEffects.Callback += SyncEffects;

        Parameters = new Dictionary<LifeCycleParameterEnum, LifeCycleParameter>() {
            { LifeCycleParameterEnum.Bleeding, bleed },
            { LifeCycleParameterEnum.Endurance, endurance },
            { LifeCycleParameterEnum.Health, health },
            { LifeCycleParameterEnum.Radiation, radiation },
            { LifeCycleParameterEnum.Satiety, satiety },
        };

        health.OnMin += Die;

        if (health.Value > health.MinValue) {
            IsAlive = true;
        } else {
            Die();
        }

        effects = new HashSet<LifeCycleEffect>();
    }

    public override void OnStartClient() {
        // When connecting a player on the server, there could already be effects
        foreach (var effect in syncEffects) {
            effects.Add(effect);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Setting and synchronizing the initial effects set in the inspector
        var permanentEffects = new LifeCycleEffect[] { regeneration, enduranceRecovery, hunger,
            radiationExcretion };
        for (int i = 0; i < permanentEffects.Length; i++) {
            AddEffectAndSetStartTime(permanentEffects[i]);
        }
    }

    private void SyncEffects(SyncHashSet<LifeCycleEffect>.Operation op,  LifeCycleEffect item) {
        switch (op) {
            case SyncHashSet<LifeCycleEffect>.Operation.OP_ADD:
            {
                effects.Add(item);
                break;
            }
            case SyncHashSet<LifeCycleEffect>.Operation.OP_REMOVE:
            {
                effects.Remove(item);
                break;
            }
        }
    }

    private void Die() {
        IsAlive = false;
        OnDeath?.Invoke();
    }

    #region Add And Remove Effects

    [Server]
    private void AddLifecycleEffect(LifeCycleEffect effect) {
        syncEffects.Add(effect);
    }

    [Command]
    private void CmdAddLifecycleEffect(LifeCycleEffect effect) {
        AddLifecycleEffect(effect);
    }

    [Server]
    private void RemoveLifecycleEffect(LifeCycleEffect effect) {
        syncEffects.Remove(effect);
    }

    [Command]
    private void CmdRemoveLifecycleEffect(LifeCycleEffect effect) {
        RemoveLifecycleEffect(effect);
    }

    public void RemoveEffect(LifeCycleEffect effect) {
        if (isServer) {
            RemoveLifecycleEffect(effect);
        } else {
            CmdRemoveLifecycleEffect(effect);
        }
    }

    /// <summary>
    /// Adds an effect and returns the same effect, but with a set start time
    /// (which was added)
    /// </summary>
    public LifeCycleEffect AddEffectAndSetStartTime(LifeCycleEffect effect) {
        effect.StartTime = NetworkTime.time;
        // Todo: not change passed effect
        if (isServer) {
            AddLifecycleEffect(effect);
        } else {
            CmdAddLifecycleEffect(effect);
        }
        return effect;
    }
    #endregion

    private void Update() {
        UpdateEffects();
    }

    private void UpdateEffects() {
        List<LifeCycleEffect> effectsToRemove = new List<LifeCycleEffect>();
        foreach (var effect in effects) {
            if (!effect.isInfinite && IsPassed(effect)) {
                // Past temporary effects are postponed for deletion
                // (can't change the dictionary while we're going through it)
                if (isServer)
                    effectsToRemove.Add(effect);
            } else {
                ApplyEffect(effect);
            }
        }
        if (isServer) {
            foreach (var effectId in effectsToRemove)
                RemoveLifecycleEffect(effectId);
        }
    }

    bool IsPassed(LifeCycleEffect effect) => effect.StartTime + effect.duration <= NetworkTime.time;

    public void ApplyEffect(LifeCycleEffect effect) {
        // If the effect is infinite or has not completed
        if ((effect.isInfinite || !IsPassed(effect))) {
            LifeCycleParameter target = Parameters[effect.targetParameterId];
            target.Value += effect.speed * Time.deltaTime;
        }
    }

    private LifeCycleEffect runEffect;

    #region Movement
    public void Run() {
        runEffect = AddEffectAndSetStartTime(runEnduranceDecrease);
    }
    public void StopRun() {
        RemoveEffect(runEffect);
    }
    #endregion
}
