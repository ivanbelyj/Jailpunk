using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPhysicalMovement))]
public class PlayerMovement : MonoBehaviour
{
    private GridPhysicalMovement movement;
    private void Awake() {
        movement = GetComponent<GridPhysicalMovement>();
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.MovementInputValues = GridUtils.RotateCartesian(
            new Vector3(horizontal, vertical, 0));
    }

    private void OnDrawGizmos() {
        Color prevColor = Gizmos.color;
        if (movement != null && movement.MovementInputValues != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, movement.MovementInputValues);
        }
        Gizmos.color = prevColor;
    }
}
