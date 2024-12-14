using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static ConnectivityDebugUtils;

public class SectorsConnectivityProcessing : GenerationStage
{
    [SerializeField]
    private bool showLogMessages = false;
    [SerializeField]
    private bool addDebugSectorBoundaryMarks = true;

    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var connectivityResult = GetSectorsConnectivity(context);
        context.SectorPossibleConnectivity = connectivityResult.ConnectivityGraph;
        context.SectorBoundaries = connectivityResult.Boundaries;
        
        HandleDebug(context);

        return context;
    }

    private ConnectivityResult GetSectorsConnectivity(GenerationContext context) {
        var connectivityProcessor = new ConnectivityProcessor();
        return connectivityProcessor.ProcessConnectivity(
            (x, y) => {
                return context.MazeData.Scheme.GetTileByPos(x, y).SectorId;
            },
            context.MazeData.Scheme.MapSize);
    }

    #region Debug
    private void HandleDebug(GenerationContext context) {
        if (showLogMessages) {
            PrintGraphToConsole(context.SectorPossibleConnectivity);
            PrintBoundariesToConsole(context.SectorBoundaries);
        }
        
        if (addDebugSectorBoundaryMarks) {
            AddBoundariesDebugMarks(context.SectorBoundaries);
        }
    }
    #endregion
}
