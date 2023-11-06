using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parameters used by BSP generator
/// </summary>
[System.Serializable]
public class BSPGenerationOptions
{
    [Tooltip("Controls frequency of splitting leaves that are met size requirements")]
    public float splitLeafProbability = 0.75f;
    // public float SplitLeafProbability => splitLeafProbability;

    [Tooltip("Used to determine whether the leaf will split horizontally or vertically")]
    public float minSplitRatio = 1.25f;
    // public float MinSplitRatio => minSplitRatio;

    public int maxRoomInnerOffset = 0;
    public int maxRoomOuterOffset = 0;

    [Header("Set dynamically")]
    public Vector2Int rootLeafSize;
    public int minLeafSize;
    public int maxLeafSize;
}
