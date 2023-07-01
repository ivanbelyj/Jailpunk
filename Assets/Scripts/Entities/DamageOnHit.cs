using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class DamageOnHit : MonoBehaviour
{
    [SerializeField]
    private float damageForceThreshold = 1f;

    [SerializeField]
    private float damageForceScale = 5f;

    private IDamageable damageable;

    private void Awake() {
        damageable = GetComponent<IDamageable>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float force = collision.relativeVelocity.sqrMagnitude;
        if (force > damageForceThreshold) {
            float damage = (force - damageForceThreshold) * damageForceScale;
            // Todo: apply damage, [0, 1]
        }
    }
}
