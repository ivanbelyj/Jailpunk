using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    private GridPhysicalMovement movement;
    [SerializeField]
    private Animator animator;

    private void Awake() {
        movement = GetComponent<GridPhysicalMovement>();

        // Initial character animation orientation
        SetAnimatorParams(Vector2.up);
    }

    private Vector2 lastMoveInput;
    private float sameMoveInputTime;
    [SerializeField]
    [Tooltip("Used to track the elapsed time " +
        "since the last change in the moveInput value. It is used to determine " +
        " when to reset the animator parameters.")]
    private float changeAnimationDirectionDuration = 0.1f;

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

        if (sameMoveInputTime >= changeAnimationDirectionDuration
            && !Mathf.Approximately(moveInput.sqrMagnitude, 0f))
        {
            SetAnimatorParams(moveInput);
        }

        movement.MovementInputValues = GridUtils.RotateCartesian(
            new Vector3(moveInput.x, moveInput.y, 0));
    }

    private void SetAnimatorParams(Vector2 moveInput) {
        float Normalize(float val) {
            if (Mathf.Abs(val) < 0.1f)
                return 0f;
            else return val < 0 ? -1f : 1f;
        }

        // Set the animator parameters when the changeDuration has elapsed
        animator.SetFloat("PosX", Normalize(moveInput.x));
        animator.SetFloat("PosY", Normalize(moveInput.y));
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
