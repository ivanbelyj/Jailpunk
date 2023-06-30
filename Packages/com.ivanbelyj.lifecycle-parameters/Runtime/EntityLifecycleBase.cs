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
public abstract class EntityLifecycleBase : NetworkBehaviour
{
    private readonly SyncHashSet<LifecycleEffect> syncEffects =
        new SyncHashSet<LifecycleEffect>();

    private HashSet<LifecycleEffect> effects;

    ///<summary>
    /// All uncompleted effects applied to change the parameters of the lifecycle.
    /// Completed effects are removed after regular traversal 
    ///</summary>
    public HashSet<LifecycleEffect> Effects => effects;

    // [SerializeField]
    // protected LifecycleParameter[] initialParameters;

    ///<summary>
    /// All lifecycle parameters collected for traversal.
    /// Dynamic addition / removal of parameters is not assumed
    ///</summary>
    protected IReadOnlyDictionary<uint, LifecycleParameter> Parameters { get; private set; }

    public virtual void Awake() {
        syncEffects.Callback += SyncEffects;
        effects = new HashSet<LifecycleEffect>();

        Dictionary<uint, LifecycleParameter> parametersDict
            = new Dictionary<uint, LifecycleParameter>();
        foreach (var parameter in GetInitialParameters()) {
            parametersDict.Add(parameter.ParameterId, parameter);
        }
        Parameters = parametersDict;
    }

    /// <summary>
    /// Derived class gets parameters from user-friendly places.
    /// Access parameters in user code via properties or fields 
    /// is more convenient than array
    /// </summary>
    public abstract LifecycleParameter[] GetInitialParameters();

    /// <summary>
    /// Derived class gets initial effects from user-friendly places.
    /// Access initial effects in user code via properties or fields 
    /// is more convenient than array
    /// </summary>
    public abstract LifecycleEffect[] GetInitialEffects();

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
        var initialEffects = GetInitialEffects();
        for (int i = 0; i < initialEffects.Length; i++) {
            AddEffectAndSetStartTime(initialEffects[i]);
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
}
