using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component of GameObject that has isometric orientation
/// </summary>
public abstract class IsometricOrientedObject : MonoBehaviour
{
    [SerializeField]
    protected IsometricDirection orientation;
    public IsometricDirection Orientation {
        get => orientation;
        set => orientation = value;
    }

    public virtual void OnDrawGizmos() {
        Color prevColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Forward);
        Gizmos.color = prevColor;
    }

    public Vector3 Forward => IsometricUtils
        .MazeObjectOrientationToVector2(orientation).normalized;

    public Vector3 Rotated90 => IsometricUtils
        .MazeObjectOrientationToRotated90Vector2(orientation).normalized;
}
