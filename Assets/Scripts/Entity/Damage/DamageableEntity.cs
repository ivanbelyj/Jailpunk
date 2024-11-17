using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

using static DamageConstants;

[RequireComponent(typeof(DestroyableLifecycle))]
public class DamageableEntity : NetworkBehaviour, IDamageable
{
    [SerializeField]
    [Tooltip("Max health in hit points")]
    private float maxHealthHP = 20f;

    private DestroyableLifecycle destroyableLifecycle;

    [SerializeField]
    private GameObject destroyOnDeath;

    [SerializeField]
    private FlashEffectData damageFlashEffectData = new() {
        tint = new Color(1f, 0.5f, 0.5f),
        flashColor = new Color(0.5f, 0, 0),
    };

    [SerializeField]
    private CustomizableAppearance customizableAppearance;

    private void Awake() {
        destroyableLifecycle = GetComponent<DestroyableLifecycle>();
        destroyableLifecycle.EntityDestroyed += OnEntityDestroyed;
    }

    public void Damage(Damage damage)
    {
        ApplyFlashEffect();
        Debug.Log($"Apply damage {damage.force}");
        destroyableLifecycle.AddEffect(ToEffect(damage));
    }

    private void ApplyFlashEffect() {
        if (customizableAppearance != null) {
            foreach (var elementGO in customizableAppearance.GetElements()) {
                elementGO
                    .GetComponent<FlashEffect>()
                    .FlashAndFade(damageFlashEffectData);
            }
        }
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
            Debug.Log("Key down");
            Damage(new Damage() {
                force = 2,
                damageType = DamageType.Punch
            });
        }
    }
}
