using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float damageForceThreshold = 1f;

    [SerializeField]
    private float damageForceScale = 5f;

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision with damageable");
        float force = collision.relativeVelocity.sqrMagnitude;
        if (force > damageForceThreshold) {
            float damage = (force - damageForceThreshold) * damageForceScale;
            Debug.Log("Damage: " + damage);
        }
    }
}
