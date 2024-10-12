using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parameters of the maze generation process
/// </summary>
[System.Serializable]
public class GenerationData
{
    [SerializeField]
    private bool useRandomSeed = false;
    public bool UseRandomSeed {
        get => useRandomSeed;
        private set => useRandomSeed = value;
    }

    [SerializeField]
    private int seed;
    public int Seed { get => seed; private set => seed = value; }

    [SerializeField]
    private Vector2Int mazeSize; 
    public Vector2Int MazeSize { get => mazeSize; set => mazeSize = value; }

    [SerializeField]
    private int minSectorSize = 6;
    public int MinSectorSize {
        get => minSectorSize;
        private set => minSectorSize = value;
    }
    [SerializeField]
    private int maxSectorSize = 16;
    public int MaxSectorSize {
        get => maxSectorSize;
        private set => maxSectorSize = value;
    }
}
