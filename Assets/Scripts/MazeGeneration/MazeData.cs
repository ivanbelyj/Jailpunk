using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Data of the maze used for generation and transferred between
/// generation stages
/// </summary>
public class MazeData
{
    public int[,] Walls { get; set; }
    // Todo:
    // public MazeScheme Scheme { get; set; }
}
