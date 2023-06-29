using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maze objects that can be activated and refresh to initial state
/// </summary>
public interface IRefreshable : IActivatable
{
    void Refresh();
}
