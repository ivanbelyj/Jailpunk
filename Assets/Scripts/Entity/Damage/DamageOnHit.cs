using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class DamageOnHit : MonoBehaviour
{
    [SerializeField]
    private float damageForceThreshold = 10f;

    [SerializeField]
    private float damageForceScale = 0.2f;

    private IDamageable damageable;

    private void Awake() {
        damageable = GetComponent<IDamageable>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float force = collision.relativeVelocity.sqrMagnitude;
        
        if (force > damageForceThreshold) {
            float damageHP = (force - damageForceThreshold) * damageForceScale;
            if (damageHP < 0) {
                damageHP = 0f;
            }

            Debug.Log("Damage on hit. Force: " + force + ", HP: " + damageHP);    
            damageable.Damage(new() {
                 damageType = DamageType.Punch,
                 force = damageHP
            });
        }
    }
}
