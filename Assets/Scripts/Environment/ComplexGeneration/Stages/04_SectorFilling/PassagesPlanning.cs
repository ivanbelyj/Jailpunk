using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ConnectivityDebugUtils;

public class PassagesPlanning : GenerationStage
{
    [SerializeField]
    private bool showLogMessages = false;

    [SerializeField]
    private bool addDebugSectorBoundaryMarks = true;

    public override void RunStage()
    {
        if (addDebugSectorBoundaryMarks) {
            AddBoundariesDebugMarks(GenerationData.AreaBoundariesAllSectors);
        }

        ApplyPassagesBetweenAreas(
            context.ComplexData.Scheme,
            context.ComplexData.Scheme.Areas,
            GenerationData.AreaBoundariesAllSectors);
    }

    private void ApplyPassagesBetweenAreas(
        ComplexScheme scheme,
        List<SchemeArea> areas,
        List<AreasBoundary> boundaries)
    {
        foreach (var boundary in boundaries) {
            var pairs = boundary.GetNeighboringBoundaryPairs();
            var (tileA, tileB) = pairs[Random.Range(0, pairs.Count)];
            scheme.GetTileByPos(tileA.x, tileA.y).Type = SchemePositionType.Floor;
            scheme.GetTileByPos(tileB.x, tileB.y).Type = SchemePositionType.Floor;
        }
    }
}
