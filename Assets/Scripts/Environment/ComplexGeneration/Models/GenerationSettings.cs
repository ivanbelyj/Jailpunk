using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generation parameters that tend not to change for different games
/// (or they are overriden by <see cref="GenerationParameters"/>)
/// </summary>
[System.Serializable]
public class GenerationSettings
{
    [Header("Common")]
    public Vector3 complexPosition;

    [Tooltip(
        "Options used for BSP maze structure generation. " +
        "Some options will be overriden in the generation")]
    public BSPGenerationOptions structureBSPOptions;

    [Header("Sector planning")]
    [Tooltip(
        "Options used for planning sectors that use SectorRoomPlanningStrategy. " +
        "Some options will be overriden in the generation")]
    public BSPGenerationOptions sectorBSPOptions;

    [Header("Subsectors")]
    [Tooltip("Max aspect ratio to plan room block as a subsector")]
    public float maxSubsectorRatio = 1.5f;

    [Tooltip("Minimal average size to plan room block as a subsector")]
    public int subsectorMinSize = 32;

    [Tooltip("Define how to plan subsectors into room blocks")]
    [SerializeField]
    public BSPGenerationOptions subsectorBSPOptions;

    [Header("Sector rooms")]
    [Tooltip("Define how to separate room blocks into rooms")]
    [SerializeField]
    public BSPGenerationOptions roomBlockBSPOptions;
}
