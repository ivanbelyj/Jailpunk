using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Data representing the maze complex.
/// It can be used in maze building on the scene.
/// This structure should not contain intermediate 
/// generation-specific data
/// </summary>
public class MazeData
{
    public MazeScheme Scheme { get; set; }
}
