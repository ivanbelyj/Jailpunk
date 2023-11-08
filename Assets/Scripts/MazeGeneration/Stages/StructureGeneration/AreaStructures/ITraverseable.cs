using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An area that can be traversed for mapping to the maze scheme
/// </summary>
public interface ITraverseable
{
    // Todo: bool isWall instead of tile type!
    public delegate void TraversalDelegate(int x, int y, bool isWall);
    void Traverse(TraversalDelegate callback);
}
