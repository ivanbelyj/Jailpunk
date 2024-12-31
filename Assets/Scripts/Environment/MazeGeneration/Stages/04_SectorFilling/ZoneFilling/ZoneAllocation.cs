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
        if (context.GeneratedSectors.Count != context.AreaPossibleConnectivityBySectorId.Count) {
            Debug.LogWarning(
                $"Generated sectors: {context.GeneratedSectors.Count}. " +
                $"Connectivity: {context.AreaPossibleConnectivityBySectorId.Count}");   
        }
        foreach (var generatedSector in context.GeneratedSectors) {
            if (!context.AreaPossibleConnectivityBySectorId.ContainsKey(generatedSector.Id)) {
                continue;
            }

            SetRequestedZones(generatedSector);
            
            AllocateZones(
                generatedSector,
                context.AreaPossibleConnectivityBySectorId[generatedSector.Id]);
        }
    }

    private void SetRequestedZones(GeneratedSectorInfo generatedSector)
    {
        var requestedSector = FindRequestedSector(generatedSector.Id);
        var (sectorZones, zoneGroups) = requestedSector == null
            ? CreateZonesForSector(generatedSector)
            : (requestedSector.Zones, requestedSector.ZoneGroups);

        generatedSector.Zones = sectorZones;
        generatedSector.ZoneGroups = zoneGroups;
    }

    private (List<ZoneInfo>, List<AllocatableAreaGroup>) CreateZonesForSector(
        GeneratedSectorInfo sector)
    {
        return (new(), new());
    }

    private SectorInfo FindRequestedSector(int generatedSectorId) =>
        context
            .Request
            .RequestedSectors
            .FirstOrDefault(x => x.GeneratedSectorId == generatedSectorId);

    private void AllocateZones(
        GeneratedSectorInfo generatedSector,
        Graph<int> sectorAreaConnectivity)
    {
        if (generatedSector.Zones == null || generatedSector.Zones.Count == 0) {
            return;
        }
        Debug.Log(
            $"Sector {generatedSector.Id} has {generatedSector.SchemeAreas.Count} areas " +
            $"and {sectorAreaConnectivity.Nodes.Count} connectivity nodes");
        if (generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count) {
            Debug.LogWarning("generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count");
        }
        var areaAllocator = new AreaAllocator();
        
        areaAllocator.AllocateAreas(
            generatedSector.Zones,
            generatedSector.ZoneGroups,
            sectorAreaConnectivity
        );

        AddDebugColorForGeneratedZones(generatedSector);
    }

    private void AddDebugColorForGeneratedZones(GeneratedSectorInfo generatedSector) {
        foreach (var zone in generatedSector.Zones) {
            if (zone.GeneratedZoneId == null) {
                continue;
            }
            Debug.Log("Add debug area color to " + zone.GeneratedZoneId);
            MazeGenerator.AddDebugAreaColor(
                zone.GeneratedZoneId.Value,
                DebugColorMarkUtils.GetNextColor());
        }
    }
}
