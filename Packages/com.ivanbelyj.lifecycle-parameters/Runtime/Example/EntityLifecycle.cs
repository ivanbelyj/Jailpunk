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
    public LifecycleParameter Health {
        get => health;
    }

    [SerializeField]
    private LifecycleParameter endurance;
    public LifecycleParameter Endurance {
        get => endurance;
    }
    
    [SerializeField]
    private LifecycleParameter satiety;
    public LifecycleParameter Satiety {
        get => satiety;
    }
    

    [SerializeField]
    private LifecycleParameter bleed;
    public LifecycleParameter Bleed {
        get => bleed;
    }
    

    [SerializeField]
    private LifecycleParameter radiation;
    public LifecycleParameter Radiation {
        get => radiation;
    }
    

    [Header("Initial effects")]
    [SerializeField]
    private LifecycleEffect regeneration;
    [SerializeField]
    private LifecycleEffect enduranceRecovery;
    [SerializeField]
    private LifecycleEffect hunger;
    [SerializeField]
    private LifecycleEffect radiationExcretion;

    // Todo: decrease of maximal health when recovering

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
            health,
            endurance,
            satiety,
            bleed,
            radiation,
        };
        return initialParameters;
    }

    public override LifecycleEffect[] GetInitialEffects()
    {
        var initialEffects = new LifecycleEffect[] {
            regeneration, enduranceRecovery, hunger,
            radiationExcretion };
        return initialEffects;
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
