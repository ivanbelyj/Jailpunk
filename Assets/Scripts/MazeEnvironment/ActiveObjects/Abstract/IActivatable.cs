using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that can be activated
/// </summary>
public interface IActivatable
{
    ActivationState State { get; }
    event Action<IActivatable, ActivationState, ActivationState>
        ActivationStateChanged;

    /// <summary>
    /// Activates object if current state allows the activation
    /// <summary>
    void ActivateIfAllowed();
}
