using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloseCombat : MonoBehaviour
{
    [SerializeField]
    private float attackDistance = 1f;

    [SerializeField]
    private float attackMinCooldown = 0.5f;

    [SerializeField]
    private float attackFullCooldown = 1.2f;

    [SerializeField]
    private float minPunchForce = 1;

    [SerializeField]
    private float maxPunchForce = 2;

    [Tooltip("Use to set the point from which the attack will be performed")]
    [SerializeField]
    private Transform anchor;

    private GridTransform gridTransform;

    private float lastAttackTime;

    private void Awake() {
        gridTransform = GetComponent<GridTransform>()
            ?? GetComponentInParent<GridTransform>();
    }

    public void Attack() {
        var currentTime = Time.time;
        if (ShouldAttack(currentTime)) {
            var hits = GetAttackRaycastHits();
            var damageables = GetDamageablesByRaycastHits(hits);
            
            foreach (var (damageable, go) in damageables) {
                var damage = CreateDamage(currentTime);
                Debug.Log("Apply damage: " + damage);

                damageable.Damage(damage);

                // ApplyKnockback(go);
            }

            lastAttackTime = currentTime;
        }
    }

    private bool ShouldAttack(float currentTime)
        => currentTime - lastAttackTime >= attackMinCooldown;

    private Damage CreateDamage(float currentTime) {
        float cooldownProgress = (currentTime - lastAttackTime - attackMinCooldown)
            / (attackFullCooldown - attackMinCooldown);
        var punchForce = Mathf.Lerp(minPunchForce, maxPunchForce, cooldownProgress);
        return new() {
            damageType = DamageType.Punch,
            force = punchForce
        };
    }

    private IEnumerable<(IDamageable, GameObject)> GetDamageablesByRaycastHits(
        IEnumerable<RaycastHit2D> hits) {
        return hits
            .Where(hit => hit.transform.root != transform.root)
            .Select(hit => (
                damageable: hit.collider.GetComponentInParent<IDamageable>(),
                go: hit.collider.gameObject))
            .Where(x => x.damageable != null);
    }

    private RaycastHit2D[] GetAttackRaycastHits() {
        var vector = GridDirectionUtils
            .GridDirectionToCartesianVector(gridTransform.Orientation)
            .normalized;
        return Physics2D.RaycastAll(
            anchor?.position ?? transform.position,
            vector,
            attackDistance);
    }

    // Todo:
    // private void ApplyKnockback(GameObject go) {
    //     Rigidbody2D rb = go.transform.parent.parent.GetComponent<Rigidbody2D>()
    //         ?? go.GetComponent<Rigidbody2D>();
    //     Debug.Log("RB (2D) to knockout: " + rb);
    //     if (rb != null) {
    //         Vector3 knockbackDirection = go.transform.position - transform.position;
    //         Debug.Log("KNOCKBACK DIRECTION: " + knockbackDirection * damage.punchForce);
    //         rb.AddForce(knockbackDirection * damage.punchForce, ForceMode2D.Force);
    //     }
    // }
}
