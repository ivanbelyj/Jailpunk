using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLifecycle : EntityLifecycleBase
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

    public event Action OnDeath;

    public bool IsAlive { get; private set; }

    public override void Awake()
    {
        base.Awake();

        health.OnMin += Die;

        if (health.Value > health.MinValue) {
            IsAlive = true;
        } else {
            Die();
        }
    }

    public override LifecycleParameter[] GetInitialParameters()
    {
        var initialParameters = new LifecycleParameter[] {
            bleed,
            health,
            radiation,
            satiety
        };
        return initialParameters;
    }

    public override LifecycleEffect[] GetInitialEffects()
    {
        var permanentEffects = new LifecycleEffect[] {
            regeneration, enduranceRecovery, hunger,
            radiationExcretion };
        return permanentEffects;
    }

    private void Die() {
        IsAlive = false;
        OnDeath?.Invoke();
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
