using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequestedSectorsAllocation : GenerationStage
{
    [SerializeField]
    private bool addAllocatedSectorColorMarks = true;

    [SerializeField]
    private Color allocatedSectorDebugMarkColor = Color.red;

    public override void ProcessMaze()
    {
        var areaAllocator = new AreaAllocator();
        areaAllocator.AllocateAreas(
            context.Request.RequestedSectors,
            context.Request.SectorGroups,
            context.SectorPossibleConnectivity
        );
        HandleDebug(context, areaAllocator);
    }

    private void HandleDebug(
        GenerationContext context,
        AreaAllocator areaAllocator)
    {
        areaAllocator.DebugVerifyResult();
        if (addAllocatedSectorColorMarks) {
            foreach (var generatedSectorId in context.Request.RequestedSectors.Select(x => x.GeneratedSectorId)) {
                if (generatedSectorId != null) {
                    MazeGenerator.AddDebugSectorColor(generatedSectorId.Value, allocatedSectorDebugMarkColor);    
                }
            }
        }
    }
}
