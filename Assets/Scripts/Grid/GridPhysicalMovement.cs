using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Component controlling physical accelerated / decelerated movement
/// in grid directions
/// </summary>
public class GridPhysicalMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Max movement speed, units per second")]
    private float maxSpeed = 1f;

    [SerializeField]
    private Rigidbody2D rigidBody;

    private GridManager gridManager;

    /// <summary>
    /// Max movement speed, units per second
    /// </summary>
    public float MaxSpeed {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    [SerializeField]
    private float acceleration = 7;

    [SerializeField]
    private float deceleration = 10;

    private Vector2 velocity = Vector3.zero;

    private Vector2 movementInputValues;

    public bool showLogMessages = false;

    /// <summary>
    /// Movement input values in cartesian coordinates
    /// </summary>
    public Vector2 MovementInputValues {
        get => movementInputValues;
    }

    private GridTransform gridTransform;

    public void SetMovementInputValues(Vector2 value) {
        movementInputValues = value;
        if (movementInputValues.sqrMagnitude > 0) {
            var orientation = GridDirectionUtils.VectorToDirection(movementInputValues);
            gridTransform?.SetOrientation(orientation, true);
        }
    }

    private void Awake()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

        gridTransform = GetComponent<GridTransform>()
            ?? GetComponentInParent<GridTransform>();

        if (gridTransform == null) {
            Debug.LogError(
                $"{nameof(GridTransform)} is required for {nameof(GridPhysicalMovement)}. " +
                $"Consider adding it to the game object (name: {gameObject.name}) " +
                $"or one of its parents.");
        }
    }

    private void FixedUpdate()
    {
        Vector3 destination = transform.position
            + (Vector3)gridManager.CartesianToGridVector(
                Time.fixedDeltaTime * velocity);

        rigidBody.MovePosition(destination);
    }

    private void LateUpdate() {
        velocity = Vector3.MoveTowards(velocity,
            MovementInputValues.normalized * acceleration,
            acceleration * Time.deltaTime);

        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }

        if (MovementInputValues.sqrMagnitude == 0f)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero,
                deceleration * Time.deltaTime);
        }

        SetMovementInputValues(new Vector2());
    }
}
