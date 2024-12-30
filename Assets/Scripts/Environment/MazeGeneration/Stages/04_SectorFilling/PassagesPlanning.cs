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

    public override void ProcessMaze()
    {
        if (addDebugSectorBoundaryMarks) {
            AddBoundariesDebugMarks(context.FullAreaBoundaries);
        }

        ApplyPassagesBetweenAreas(
            context.MazeData.Scheme,
            context.MazeData.Scheme.Areas,
            context.FullAreaBoundaries);
    }

    private void ApplyPassagesBetweenAreas(
        MazeScheme scheme,
        List<SchemeArea> areas,
        List<AreasBoundary> boundaries)
    {
        foreach (var boundary in boundaries) {
            var pairs = boundary.GetNeighboringBoundaryPairs();
            var (tileA, tileB) = pairs[Random.Range(0, pairs.Count)];
            scheme.GetTileByPos(tileA.x, tileA.y).TileType = TileType.Floor;
            scheme.GetTileByPos(tileB.x, tileB.y).TileType = TileType.Floor;
        }
    }
}
