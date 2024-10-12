using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State of activatable object that indicates whether
/// it is possible to activate or to interact
/// </summary>
public enum ActivationState
{
    // /// <summary>
    // /// The interaction with activatable object is not obvious for interactor.
    // /// The object cannot be interacted and visual effects will not applied
    // /// </summary>
    // Unobvious,
    // /// <summary>
    // /// Object can be activated by interactor
    // /// </summary>
    // AcceptableForInteractor,
    // /// <summary>
    // /// Object is activating
    // /// </summary>
    // Activating,
    // /// <summary>
    // /// The conditions of interaction with the activatable object are not met
    // /// </summary>
    // UnacceptableForInteractor,
    ReadyToActivate,
    Activating,
    // Activated,
    UnableToActivate,
}
