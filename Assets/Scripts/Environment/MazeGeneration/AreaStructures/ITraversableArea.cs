using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An area that can be traversed for mapping to the maze scheme
/// </summary>
public interface ITraversableArea
{
    public delegate void TraverseDelegate(int x, int y, bool isWall);
    void Traverse(TraverseDelegate callback);
}
