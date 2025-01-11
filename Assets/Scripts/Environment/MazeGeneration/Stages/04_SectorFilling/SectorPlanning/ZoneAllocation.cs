using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneAllocation : GenerationStage
{
    [SerializeField]
    private bool showDebugMessages = true;

    public override void ProcessMaze()
    {
        if (GenerationData.GeneratedSectors.Count != GenerationData.AreaPossibleConnectivityBySectorId.Count)
        {
            Debug.LogWarning(
                $"Generated sectors: {GenerationData.GeneratedSectors.Count}. " +
                $"Connectivity: {GenerationData.AreaPossibleConnectivityBySectorId.Count}");   
        }
        foreach (var generatedSector in GenerationData.GeneratedSectors)
        {
            if (!GenerationData.AreaPossibleConnectivityBySectorId.ContainsKey(generatedSector.Id)) {
                continue;
            }

            // SetRequestedZones(generatedSector);

            var requestedSector = FindRequestedSector(generatedSector.Id);
            if (requestedSector == null) {
                continue;
            }
            
            AllocateZones(
                generatedSector,
                requestedSector,
                GenerationData.AreaPossibleConnectivityBySectorId[generatedSector.Id]);
        }
    }

    private SectorInfo FindRequestedSector(int generatedSectorId) =>
        context
            .Request
            .RequestedSectors
            .FirstOrDefault(x => x.GeneratedSectorId == generatedSectorId);

    private void AllocateZones(
        GeneratedSectorInfo generatedSector,
        SectorInfo sectorInfo,
        Graph<int> sectorAreaConnectivity)
    {
        // if (generatedSector.RequestedZones == null || generatedSector.RequestedZones.Count == 0) {
        //     return;
        // }
        Debug.Log(
            $"Sector {generatedSector.Id} has {generatedSector.SchemeAreas.Count} areas " +
            $"and {sectorAreaConnectivity.Nodes.Count} connectivity nodes");
        if (generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count) {
            Debug.LogWarning("generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count");
        }
        var areaAllocator = CreateAllocator(generatedSector);
        
        areaAllocator.AllocateAreas(
            sectorInfo.Zones,
            sectorInfo.ZoneGroups,
            sectorAreaConnectivity
        );

        // Temporary solution ?
        // ====================
        generatedSector.Zones = new();
        foreach (var requestedZone in sectorInfo.Zones.Where(x => x.GeneratedZoneId.HasValue)) {
            generatedSector.Zones.Add(new() {
                SchemeAreaId = requestedZone.GeneratedZoneId.Value
            });
        }
        generatedSector.ZoneGroups = sectorInfo.ZoneGroups.ToList();
        // =======================

        AddDebugColorForGeneratedZones(generatedSector);
    }

    private AreaAllocator CreateAllocator(GeneratedSectorInfo generatedSector) {
        var areaFilter = new ZoneAreaAllocatorFilter(generatedSector.SchemeAreas);
        return new AreaAllocator(areaFilter);
    }

    private void AddDebugColorForGeneratedZones(GeneratedSectorInfo generatedSector)
    {
        foreach (var zone in generatedSector.Zones) {
            MazeGenerator.AddDebugAreaColor(
                zone.SchemeAreaId,
                DebugColorMarkUtils.GetNextColor());
        }
    }
}
