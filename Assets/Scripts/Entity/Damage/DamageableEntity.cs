using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static DamageConstants;

[RequireComponent(typeof(DestroyableLifecycle))]
public class DamageableEntity : MonoBehaviour, IDamageable
{
    [SerializeField]
    [Tooltip("Max health in hit points")]
    private float maxHealthHP = 20f;

    private DestroyableLifecycle destroyableLifecycle;

    [SerializeField]
    private GameObject destroyOnDeath;

    private void Awake() {
        destroyableLifecycle = GetComponent<DestroyableLifecycle>();

        destroyableLifecycle.EntityDestroyed += OnEntityDestroyed;
    }

    public void Damage(Damage damage)
    {
        destroyableLifecycle.AddEffectAndSetStartTime(ToEffect(damage));
    }

    /// <summary>
    /// The damage that the entity actually receives
    /// </summary>
    protected virtual Damage GetAppliedDamage(Damage damage) {
        // Do nothing by default
        return damage;
    }

    private LifecycleEffect ToEffect(Damage damage) {
        return LifecycleEffect.CreateForDelta(
            LifecycleParameterIds.Strength,
            delta: -ToLifecycleParameterDelta(damage.force),
            duration: DefaultHitEffectDuration
        );
    }

    private float ToLifecycleParameterDelta(float hp) => hp / maxHealthHP;

    private void OnEntityDestroyed() {
        Destroy(destroyOnDeath);
    }

    // For debug
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            Damage(new Damage() {
                force = 2,
                damageType = DamageType.Punch
            });
        }
    }
}
