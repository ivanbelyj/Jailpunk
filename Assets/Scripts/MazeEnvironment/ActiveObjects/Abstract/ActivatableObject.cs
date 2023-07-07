using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that can be activated
/// </summary>
public abstract class ActivatableObject : MonoBehaviour, IActivatable
{
    [SerializeField]
    private ActivationState state = ActivationState.ReadyToActivate;
    public ActivationState State {
        get => state;
        protected set {
            if (value == state)
                return;
            var oldState = state;
            state = value;
            ActivationStateChanged?.Invoke(this, oldState, state);
        }
    }

    /// <summary>
    /// Fires when state of activatable object is changed.
    /// First argument - previous state, second - new state
    /// </summary>
    public event Action<IActivatable, ActivationState, ActivationState>
        ActivationStateChanged;

    public virtual void ActivateIfAllowed() {
        if (State == ActivationState.ReadyToActivate) {
            Activate();
        }
    }

    protected abstract void Activate();
}
