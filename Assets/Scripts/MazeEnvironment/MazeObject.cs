using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeObject : MonoBehaviour, IActivatable
{
    [SerializeField]
    protected MazeObjectOrientation orientation;

    private ActivatableState state = ActivatableState.ReadyToActivate;
    public ActivatableState State {
        get => state;
        protected set {
            var oldState = state;
            if (value == state)
                return;
            state = value;
            OnStateChanged?.Invoke(oldState, state);
        }
    }

    /// <summary>
    /// Fires when state of activatable object is changed.
    /// First argument - previous state, second - new state
    /// </summary>
    public event Action<ActivatableState, ActivatableState> OnStateChanged;

    public abstract void Activate();

    private void OnDrawGizmos() {
        Color prevColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, IsometricUtils
            .MazeObjectOrientationToVector2(orientation).normalized);
        Gizmos.color = prevColor;
    }
}
