using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maze objects that can be activated in some way
/// </summary>
public interface IActivatable
{
    ActivatableState State { get; }
    void Activate();
}
