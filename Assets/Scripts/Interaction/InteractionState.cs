using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State of interactable object that indicates whether
/// it is possible to interact
/// </summary>
public enum InteractionState
{
    /// <summary>
    /// Object can be interacted
    /// </summary>
    Acceptable,
    /// <summary>
    /// Object is interacting
    /// </summary>
    Interacting,
    /// <summary>
    /// The conditions of interaction with the object are not met
    /// </summary>
    Unacceptable,
    /// <summary>
    /// The interaction is not obvious and the object cannot be interacted
    /// </summary>
    Unobvious
}
