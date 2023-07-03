using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridPhysicalMovement))]
public class PlayerMovement : MonoBehaviour
{
    private GridPhysicalMovement movement;
    private Vector2 moveInput;
    private void Awake() {
        movement = GetComponent<GridPhysicalMovement>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        Move(moveInput);
    }

    private void Move(Vector2 moveInput) {
        movement.MovementInputValues = GridUtils.RotateCartesian(
            new Vector3(moveInput.x, moveInput.y, 0));
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
