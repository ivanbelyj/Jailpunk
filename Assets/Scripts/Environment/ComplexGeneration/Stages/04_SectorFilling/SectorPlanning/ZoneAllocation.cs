using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneAllocation : GenerationStage
{
    [SerializeField]
    private bool showDebugMessages = true;

    public override void RunStage()
    {
        LogWarning();

        AllocateZonesForGeneratedSectors();
    }

    private void LogWarning() {
        if (GenerationData.GeneratedSectors.Count != GenerationData.AreaPossibleConnectivityBySectorId.Count)
        {
            Debug.LogWarning(
                $"Generated sectors: {GenerationData.GeneratedSectors.Count}. " +
                $"Connectivity: {GenerationData.AreaPossibleConnectivityBySectorId.Count}");   
        }
    }

    private void AllocateZonesForGeneratedSectors() {
        foreach (var generatedSector in GenerationData.GeneratedSectors)
        {
            if (!GenerationData.AreaPossibleConnectivityBySectorId.TryGetValue(
                generatedSector.Id,
                out var sectorAreaConnectivity))
            {
                continue;
            }
            else
            {
                AllocateZonesForGeneratedSector(generatedSector, sectorAreaConnectivity);
            }
        }
    }

    private void AllocateZonesForGeneratedSector(
        GeneratedSectorInfo generatedSector,
        Graph<int> sectorAreaConnectivity)
    {
        var requestedSector = FindRequestedSector(generatedSector.Id);
        if (requestedSector != null)
        {
            HandleRequestedSector(requestedSector, generatedSector, sectorAreaConnectivity);
        }
    }

    private void HandleRequestedSector(
        SectorInfo requestedSector,
        GeneratedSectorInfo generatedSector,
        Graph<int> sectorAreaConnectivity)
    {
        if (requestedSector.Zones != null) {
            Debug.Log(
                $"Sector {generatedSector.Id} has {generatedSector.SchemeAreas.Count} areas " +
                $"and {sectorAreaConnectivity.Nodes.Count} connectivity nodes");
            if (generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count) {
                Debug.LogWarning("generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count");
            }

            AllocateRequestedZones(
                generatedSector,
                requestedSector.Zones,
                requestedSector.SectorRequest.ZoneGroups,
                sectorAreaConnectivity,
                requestedSector.SectorRequest.AreaAllocationRequest.Id);
        }
    }

    private SectorInfo FindRequestedSector(int generatedSectorId) =>
        context
            .Request
            .RequestedSectors
            .FirstOrDefault(x => GenerationData.GetGeneratedSectorId(x) == generatedSectorId);

    private void AllocateRequestedZones(
        GeneratedSectorInfo generatedSector,
        List<ZoneInfo> zones,
        List<AllocatableAreaGroup> zoneGroups,
        Graph<int> sectorAreaConnectivity,
        int sectorRequestId)
    {
        var areaAllocator = CreateAllocator(generatedSector);

        var generatedZoneIdsByAllocationRequestId = areaAllocator.AllocateAreas(
            zones.Select(x => x.ZoneRequest.AreaAllocationRequest),
            zoneGroups,
            sectorAreaConnectivity
        ).GeneratedAreaIdsByAllocationRequestId;

        GenerationData.GeneratedZoneIdsByAllocationRequestIdBySectorRequestId.Add(
            sectorRequestId,
            generatedZoneIdsByAllocationRequestId
        );

        generatedSector.Zones = new();

        // Get requested zones that were allocated and add them to GeneratedSector
        var allocatedZones = zones.Where(
            x => GenerationData.GetGeneratedZoneId(x, sectorRequestId).HasValue);
        foreach (var requestedZone in allocatedZones)
        {
            generatedSector.Zones.Add(new() {
                SchemeAreaId = GenerationData
                    .GetGeneratedZoneId(requestedZone, sectorRequestId)
                    .Value
            });
        }

        AddDebugColorForGeneratedZones(generatedSector);
    }

    private AreaAllocator CreateAllocator(GeneratedSectorInfo generatedSector) {
        var areaFilter = new ZoneAreaAllocatorFilter(generatedSector.SchemeAreas);
        return new AreaAllocator(areaFilter);
    }

    private void AddDebugColorForGeneratedZones(GeneratedSectorInfo generatedSector)
    {
        foreach (var zone in generatedSector.Zones) {
            ComplexGenerator.AddDebugAreaColor(
                zone.SchemeAreaId,
                DebugColorMarkUtils.GetNextColor());
        }
    }
}
