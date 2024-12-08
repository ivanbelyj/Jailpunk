using System;
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

    // Todo: tooltip
    public bool splitMinimalLeaves = false;

    [Tooltip("Used to determine whether the leaf will split horizontally or vertically")]
    public float minSplitRatio = 1.25f;

    public int maxRoomOffset = 0;
    public int minRoomOffset = 0;

    public int leafAreasOffset = 0;

    [Tooltip("Split will be applied to nodes with max split ratio only")]
    public bool useMaxSplitRatio = false;
    public float maxSplitRatio = 1.5f;

    public Vector2Int RootNodeSize { get; set; }
    public Vector2Int RootNodeOffset { get; set; }

    [Header("Can be set dynamically in some cases")]
    public int minLeafSize = 8;
    public int maxLeafSize = 32;

    public BSPGenerationOptions Clone() {
        return (BSPGenerationOptions)MemberwiseClone();
    }
}
