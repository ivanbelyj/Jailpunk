using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Movement speed, units per second")]
    private float maxSpeed = 1f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    /// <summary>
    /// Movement input values in cartesian coordinates
    /// </summary>
    public Vector2 MovementInputValues { get; set; }

    private void FixedUpdate()
    {
        Vector3 destination = transform.position
            + IsometricUtils.CartesianToIsometric(
                Time.fixedDeltaTime * velocity);

        rb.MovePosition(destination);
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

        MovementInputValues = new Vector2();
    }
}
