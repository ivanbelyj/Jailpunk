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
public class EntityLifecycle : NetworkBehaviour
{
    #region Parameters and effects
    [Header("Parameters")]
    [SerializeField]
    private LifecycleParameter health;
    [SerializeField]
    private LifecycleParameter endurance;
    [SerializeField]
    private LifecycleParameter satiety;
    [SerializeField]
    private LifecycleParameter bleed;
    [SerializeField]
    private LifecycleParameter radiation;

    [Header("Constant effects")]
    [SerializeField]
    private LifecycleEffect regeneration;
    [SerializeField]
    private LifecycleEffect enduranceRecovery;
    [SerializeField]
    private LifecycleEffect hunger;
    [SerializeField]
    private LifecycleEffect radiationExcretion;

    [Header("Temporary effects")]
    [SerializeField]
    private LifecycleEffect runEnduranceDecrease;
    [SerializeField]
    private LifecycleEffect bleedEffect;
    [SerializeField]
    private LifecycleEffect radiationPoisoning;
    #endregion

    private readonly SyncHashSet<LifecycleEffect> syncEffects =
        new SyncHashSet<LifecycleEffect>();

    private HashSet<LifecycleEffect> effects;

    ///<summary>
    /// All uncompleted effects applied to change the parameters of the life cycle.
    /// Completed effects are removed after regular traversal 
    ///</summary>
    public HashSet<LifecycleEffect> Effects => effects;

    ///<summary>
    /// All lifecycle parameters collected for convenient traversal.
    /// Dynamic addition / removal of parameters is not assumed
    ///</summary>
    public IReadOnlyDictionary<LifecycleParameterEnum, LifecycleParameter> Parameters { get; private set; }

    public bool IsAlive { get; private set; }
    public event UnityAction OnDeath;

    private void Awake() {
        syncEffects.Callback += SyncEffects;

        Parameters = new Dictionary<LifecycleParameterEnum, LifecycleParameter>() {
            { LifecycleParameterEnum.Bleeding, bleed },
            { LifecycleParameterEnum.Endurance, endurance },
            { LifecycleParameterEnum.Health, health },
            { LifecycleParameterEnum.Radiation, radiation },
            { LifecycleParameterEnum.Satiety, satiety },
        };

        health.OnMin += Die;

        if (health.Value > health.MinValue) {
            IsAlive = true;
        } else {
            Die();
        }

        effects = new HashSet<LifecycleEffect>();
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
        var permanentEffects = new LifecycleEffect[] { regeneration, enduranceRecovery, hunger,
            radiationExcretion };
        for (int i = 0; i < permanentEffects.Length; i++) {
            AddEffectAndSetStartTime(permanentEffects[i]);
        }
    }

    private void SyncEffects(SyncHashSet<LifecycleEffect>.Operation op,  LifecycleEffect item) {
        switch (op) {
            case SyncHashSet<LifecycleEffect>.Operation.OP_ADD:
            {
                effects.Add(item);
                break;
            }
            case SyncHashSet<LifecycleEffect>.Operation.OP_REMOVE:
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
    private void AddLifecycleEffect(LifecycleEffect effect) {
        syncEffects.Add(effect);
    }

    [Command]
    private void CmdAddLifecycleEffect(LifecycleEffect effect) {
        AddLifecycleEffect(effect);
    }

    [Server]
    private void RemoveLifecycleEffect(LifecycleEffect effect) {
        syncEffects.Remove(effect);
    }

    [Command]
    private void CmdRemoveLifecycleEffect(LifecycleEffect effect) {
        RemoveLifecycleEffect(effect);
    }

    public void RemoveEffect(LifecycleEffect effect) {
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
    public LifecycleEffect AddEffectAndSetStartTime(LifecycleEffect effect) {
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
        List<LifecycleEffect> effectsToRemove = new List<LifecycleEffect>();
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

    bool IsPassed(LifecycleEffect effect) => effect.StartTime + effect.duration <= NetworkTime.time;

    public void ApplyEffect(LifecycleEffect effect) {
        // If the effect is infinite or has not completed
        if ((effect.isInfinite || !IsPassed(effect))) {
            LifecycleParameter target = Parameters[effect.targetParameterId];
            target.Value += effect.speed * Time.deltaTime;
        }
    }

    private LifecycleEffect runEffect;

    #region Movement
    public void Run() {
        runEffect = AddEffectAndSetStartTime(runEnduranceDecrease);
    }
    public void StopRun() {
        RemoveEffect(runEffect);
    }
    #endregion
}
