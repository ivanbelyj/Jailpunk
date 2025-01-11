using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Data representing the maze complex.
/// It can be used in maze building on the scene.
/// </summary>
public class MazeData
{
    public MazeScheme Scheme { get; set; }

    /// <summary>
    /// Data, used during the generation. Use carefully,
    /// it's intermediate generation-specific data
    /// </summary>
    public GenerationData GenerationData { get; set; } = new();
}
