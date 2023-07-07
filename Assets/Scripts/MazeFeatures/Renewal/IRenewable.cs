using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maze object that can renew to initial state
/// </summary>
public interface IRenewable
{
    void Renew();
    bool IsInRenewedState { get; }
}
