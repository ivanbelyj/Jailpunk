using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parameters defined outside the maze generation process
/// </summary>
[System.Serializable]
public class GenerationParameters
{
    [Tooltip("0 to use random seed")]
    [SerializeField] private int seed;
    public int Seed { get => seed; set => seed = value; }

    [SerializeField] private bool useRandomSeed;
    public bool UseRandomSeed {
        get => seed == 0 || useRandomSeed;
    }

    [SerializeField]
    private Vector2Int mazeSize;
    public Vector2Int MazeSize { get => mazeSize; set => mazeSize = value; }

    [SerializeField]
    private int minSectorSize = 15;
    public int MinSectorSize {
        get => minSectorSize;
        set => minSectorSize = value;
    }

    [SerializeField]
    private int maxSectorSize = 60;
    public int MaxSectorSize {
        get => maxSectorSize;
        set => maxSectorSize = value;
    }
}
