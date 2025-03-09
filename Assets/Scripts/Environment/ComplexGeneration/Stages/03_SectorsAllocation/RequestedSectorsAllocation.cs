using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Todo: rename ? It does not only allocate, but also selects
[RequireComponent(typeof(ISectorSelector))]
public class RequestedSectorsAllocation : GenerationStage
{
    [SerializeField]
    private bool addAllocatedSectorColorMarks = true;

    [SerializeField]
    private Color allocatedSectorDebugMarkColor = Color.red;

    private ISectorSelector sectorSelector;

    private void Awake() {
        sectorSelector = GetComponent<ISectorSelector>();
    }

    public override void RunStage()
    {
        var areaAllocator = SetSectorsAllocated();

        ApplyAllocationToGeneratedSectors();

        HandleDebug(context, areaAllocator);
    }

    private AreaAllocator SetSectorsAllocated() {
        var areaAllocator = new AreaAllocator();
        GenerationData.GeneratedSectorIdsByAllocationRequestId = areaAllocator.AllocateAreas(
            context.Request.RequestedSectors.Select(x => x.AreaAllocationRequest),
            context.Request.SectorGroups,
            GenerationData.SectorPossibleConnectivity
        ).GeneratedAreaIdsByAllocationRequestId;

        return areaAllocator;
    }

    private void ApplyAllocationToGeneratedSectors() {
        foreach (var generatedSector in GenerationData.GeneratedSectors) {
            var sectorPlanningInfo = GetSectorRequest(generatedSector.Id)?.SectorPlanningInfo
                ?? sectorSelector.GenerateSectorPlanningInfo(generatedSector, context.Request);
            ApplyAllocation(generatedSector, sectorPlanningInfo);
        }
    }

    // TODO: use better solution
    private SectorRequest GetSectorRequest(int generatedSectorId) {
        var allocationRequestId = GenerationData
            .GeneratedSectorIdsByAllocationRequestId
            .FirstOrDefault(x => x.Value == generatedSectorId).Key;
        if (allocationRequestId == default) {
            return null;
        }
        return context
            .Request
            .RequestedSectors
            .FirstOrDefault(x => x.AreaAllocationRequest.Id == allocationRequestId);
    }

    private void ApplyAllocation(
        GeneratedSectorInfo generatedSector,
        SectorPlanningInfo sectorPlanningInfo)
    {
        generatedSector.SectorPlanningInfo = sectorPlanningInfo;
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
