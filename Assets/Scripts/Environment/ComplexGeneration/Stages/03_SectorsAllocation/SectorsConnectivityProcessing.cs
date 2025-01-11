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

    public override void RunStage()
    {
        var connectivityResult = GetSectorsConnectivity(context);
        GenerationData.SectorPossibleConnectivity = connectivityResult.ConnectivityGraph;
        GenerationData.SectorBoundaries = connectivityResult.Boundaries;
        
        HandleDebug(context);
    }

    private ConnectivityResult GetSectorsConnectivity(GenerationContext context) {
        var connectivityProcessor = new ConnectivityProcessor();
        return connectivityProcessor.ProcessConnectivity(
            (x, y) => {
                return context.ComplexData.Scheme.GetTileByPos(x, y).SectorId;
            },
            context.ComplexData.Scheme.MapSize);
    }

    #region Debug
    private void HandleDebug(GenerationContext context) {
        if (showLogMessages) {
            PrintGraphToConsole(GenerationData.SectorPossibleConnectivity);
            PrintBoundariesToConsole(GenerationData.SectorBoundaries);
        }
        
        if (addDebugSectorBoundaryMarks) {
            AddBoundariesDebugMarks(GenerationData.SectorBoundaries);
        }
    }
    #endregion
}
