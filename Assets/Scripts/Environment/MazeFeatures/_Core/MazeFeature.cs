using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// A feature that the maze may have in some generations,
/// or that may be disabled in other
/// </summary>
public abstract class MazeFeature : NetworkBehaviour
{
    [SerializeField]
    private bool isEnabled;
    public bool IsEnabled => isEnabled;

    protected Maze maze;

    public void Initialize(Maze maze) {
        this.maze = maze;
    }
}
