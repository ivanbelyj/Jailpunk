using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

/// <summary>
/// Controls of character movement
/// </summary>
[RequireComponent(typeof(GridPhysicalMovement))]
[RequireComponent(typeof(SpriteSwapAnimator))]
public class CharacterControls : MonoBehaviour
{
    private GridPhysicalMovement movement;
    private SpriteSwapAnimator animator;

    private GridManager gridManager;

    private void Awake() {
        movement = GetComponent<GridPhysicalMovement>();
        animator = GetComponent<SpriteSwapAnimator>();
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    private Vector2 lastMoveInput;
    private Vector2 currentOrientationMoveInput;
    private float sameMoveInputTime;
    [SerializeField]
    [Tooltip("Used to track the elapsed time " +
        "since the last change in the moveInput value. It is used to determine " +
        " when to reset the animator parameters.")]
    private float changeAnimationDirectionDuration = 0.1f;

    // Todo: initial orientation
    public Vector2 Orientation =>
        gridManager.CartesianToVector2(
            GridUtils.RotateCartesian(currentOrientationMoveInput)
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
