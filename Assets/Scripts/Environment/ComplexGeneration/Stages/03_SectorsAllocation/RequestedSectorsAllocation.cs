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

    public override void RunStage()
    {
        var areaAllocator = new AreaAllocator();
        GenerationData.GeneratedSectorIdsByAllocationRequestId = areaAllocator.AllocateAreas(
            context.Request.RequestedSectors.Select(x => x.SectorRequest.AreaAllocationRequest),
            context.Request.SectorGroups,
            GenerationData.SectorPossibleConnectivity
        ).GeneratedAreaIdsByAllocationRequestId;
        HandleDebug(context, areaAllocator);
    }

    private void HandleDebug(
        GenerationContext context,
        AreaAllocator areaAllocator)
    {
        areaAllocator.DebugVerifyResult();
        if (addAllocatedSectorColorMarks) {
            foreach (var generatedSectorId in
                context.Request.RequestedSectors.Select(x => GenerationData.GetGeneratedSectorId(x)))
            {
                if (generatedSectorId != null) {
                    ComplexGenerator.AddDebugSectorColor(
                        generatedSectorId.Value,
                        allocatedSectorDebugMarkColor);    
                }
            }
        }
    }
}
