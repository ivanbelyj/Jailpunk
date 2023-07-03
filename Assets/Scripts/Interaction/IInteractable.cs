using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that can be interacted by Interactor
/// </summary>
public interface IInteractable
{
    InteractionState State { get; }

    /// <summary>
    /// Sets visual effects according to the state.
    /// Clear effects if val is false
    /// </summary>
    void SetVisualEffectsForState(bool val);
}
