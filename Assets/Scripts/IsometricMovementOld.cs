using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricMovementOld : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Movement speed, units per second")]
    private float speed = 1f;

    /// <summary>
    /// Movement speed, units per second
    /// </summary>
    public float Speed {
        get => speed;
        set => speed = value;
    }

    /// <summary>
    /// Movement input values in cartesian coordinates
    /// </summary>
    public Vector2 MovementInputValues { get; set; }
    
    private void Update() {
        Vector3 delta = MovementInputValues.normalized * speed
            * Time.deltaTime;
        transform.position += IsometricUtils.CartesianToIsometric(delta);
    }
}
