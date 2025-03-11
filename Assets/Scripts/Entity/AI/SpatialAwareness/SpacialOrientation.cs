using Mirror;
using UnityEngine;

using static PerceptionEntryMarkers;

/// <summary>
/// Allows characters to memorize / recall places to orient in the game world
/// </summary>
[RequireComponent(typeof(MemorySimulator))]
public class SpacialOrientation<TPosition> : MonoBehaviour
    where TPosition : struct
{
    public const string PositionPerceptionDataKey = "Position";

    private MemorySimulator memorySimulator;

    private void Awake()
    {
        memorySimulator = GetComponent<MemorySimulator>();
    }

    public void MemorizeNewPlace(
        string perceptionVerbalRepresentation,
        float retentionIntensity,
        TPosition gridPosition,
        params string[] markers)
    {
        MemorizePlace(
            new PerceptionEntry(perceptionVerbalRepresentation, GameTimeProvider.GameTime)
            {
                Accessibility = 1f,
                RetentionIntensity = retentionIntensity
            },
            gridPosition,
            markers);
    }

    public void MemorizePlace(
        PerceptionEntry perceptionEntry,
        TPosition position,
        params string[] markers)
    {
        SetSpatialData(perceptionEntry, position, markers);
        memorySimulator.AddMemory(perceptionEntry);
    }

    private void SetSpatialData(
        PerceptionEntry perceptionEntry,
        TPosition position,
        params string[] markers)
    {
        perceptionEntry.PerceptionData[PositionPerceptionDataKey] = position;
        perceptionEntry.Markers.Add(Place);
        foreach (var marker in markers) 
        {
            perceptionEntry.Markers.Add(marker);
        }
    }
}
