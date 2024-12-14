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

    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var connectivityResult = GetAreasConnectivity(context);

        if (addDebugSectorBoundaryMarks) {
            AddBoundariesDebugMarks(connectivityResult.Boundaries);
        }

        ApplyPassagesBetweenAreas(
            context.MazeData.Scheme,
            context.MazeData.Scheme.Areas,
            connectivityResult);

        return context;
    }

    private ConnectivityResult GetAreasConnectivity(GenerationContext context) {
        var connectivityProcessor = new ConnectivityProcessor();
        return connectivityProcessor.ProcessConnectivity(
            (x, y) => {
                return context.MazeData.Scheme.GetTileByPos(x, y).AreaId;
            },
            context.MazeData.Scheme.MapSize);
    }

    private void ApplyPassagesBetweenAreas(
        MazeScheme scheme,
        List<SchemeArea> areas,
        ConnectivityResult connectivityResult)
    {
        foreach (var boundary in connectivityResult.Boundaries) {
            var pairs = boundary.GetNeighboringBoundaryPairs();
            var (tileA, tileB) = pairs[Random.Range(0, pairs.Count)];
            scheme.GetTileByPos(tileA.x, tileA.y).TileType = TileType.Floor;
            scheme.GetTileByPos(tileB.x, tileB.y).TileType = TileType.Floor;
        }
    }
}
