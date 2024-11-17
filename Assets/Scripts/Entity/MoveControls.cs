using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// GridPhysicalMovement decorator. Controls movement of an entity
/// </summary>
[RequireComponent(typeof(GridPhysicalMovement))]
public class MoveControls : MonoBehaviour, IMoveControls
{
    [SerializeField]
    private SpriteSwapAnimator animator;

    [SerializeField]
    [Tooltip(
        "Used to track the elapsed time " +
        "since the last change in the moveInput value. It is used to determine " +
        "when to reset the animator parameters.")]
    private float changeAnimationDirectionDuration = 0.1f;

    private GridPhysicalMovement movement;
    private GridManager gridManager;

    private Vector2 lastMoveInput;
    private Vector2 currentOrientationMoveInput;
    private float sameMoveInputTime;

    // Todo: initial orientation
    private Vector2 Orientation =>
        gridManager.CartesianToGridVector(
            currentOrientationMoveInput
            // Isometric: 
            // GridDirectionUtils.RotateCartesian(currentOrientationMoveInput)
        ).normalized;

    public void Move(Vector2 moveInput)
    {
        if (moveInput != lastMoveInput)
        {
            // Reset the timer if the moveInput has changed
            sameMoveInputTime = 0;
            lastMoveInput = moveInput;
        }
        else
        {
            // Increment the timer if the moveInput remains the same
            sameMoveInputTime += Time.deltaTime;
        }

        if (sameMoveInputTime >= changeAnimationDirectionDuration)
            // && !Mathf.Approximately(moveInput.sqrMagnitude, 0f))
        {
            if (!Mathf.Approximately(moveInput.sqrMagnitude, 0))
                currentOrientationMoveInput = moveInput;
            animator.SetMoveInput(moveInput);
        }

        movement.SetMovementInputValues(moveInput);
        // Isometric:
        // GridDirectionUtils.RotateCartesian(
        //     new Vector3(moveInput.x, moveInput.y, 0));
    }

    private void Awake() {
        movement = GetComponent<GridPhysicalMovement>();
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
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
