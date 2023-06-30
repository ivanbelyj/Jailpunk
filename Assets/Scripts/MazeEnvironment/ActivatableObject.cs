using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for maze objects that can be activated
/// </summary>
public abstract class ActivatableObject : MonoBehaviour, IActivatable
{
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
}
