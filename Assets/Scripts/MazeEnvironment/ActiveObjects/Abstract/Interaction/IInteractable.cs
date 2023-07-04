using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activatable object that can be interacted by Interactor
/// </summary>
public interface IInteractable : IActivatable
{
    /// <summary>
    /// Only obvious interactable object apply visual effects and can be interacted
    /// </summary>
    bool IsObvious { get; }

    event Action<IInteractable, bool, bool> ObviousnessChanged;

    /// <summary>
    /// Sets visual effects according to the state
    /// </summary>
    void SetVisualEffectsForCurrentState();

    void ClearVisualEffects();
}
