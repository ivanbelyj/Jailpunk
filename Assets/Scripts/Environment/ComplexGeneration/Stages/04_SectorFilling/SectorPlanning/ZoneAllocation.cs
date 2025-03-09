using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IZoneSelector))]
public class ZoneAllocation : GenerationStage
{
    [SerializeField]
    private bool showDebugMessages = true;

    [SerializeField]
    private bool addDebugColorForGeneratedZones = true;

    private IZoneSelector zoneSelector;

    /// <summary>
    /// Set on <see cref="SetAllocatedRequestedZones"/> 
    /// </summary>
    private Dictionary<int, ZoneRequest> requestedZonesBySchemeAreaId = new();

    private void Awake() {
        zoneSelector = GetComponent<IZoneSelector>();
    }

    public override void RunStage()
    {
        LogWarning();

        // Sectors have areas after planning.
        // We should allocate zones between areas
        AllocateZonesForGeneratedSectors();

        // Use allocation results of requested zones or use other
        // characteristics
        ApplyAllocationToGeneratedZones();
    }

    #region Allocate
    private void AllocateZonesForGeneratedSectors() {
        foreach (var generatedSector in GenerationData.GeneratedSectors)
        {
            if (GenerationData.AreaPossibleConnectivityBySectorId.TryGetValue(
                generatedSector.Id,
                out var sectorAreaConnectivity))
            {
                var requestedSector = FindRequestedSector(generatedSector.Id);

                if (requestedSector != null) {
                    SetAllocatedRequestedZonesForSector(
                        requestedSector,
                        generatedSector,
                        sectorAreaConnectivity);
                }
            } else {
                // Should never happen
                Debug.LogError("Sector area connectivity not found");
            }
        }
    }

    private void SetAllocatedRequestedZonesForSector(
        SectorRequest requestedSector,
        GeneratedSectorInfo generatedSector,
        Graph<int> sectorAreaConnectivity)
    {
        if (requestedSector.Zones != null) {
            // Debug.Log(
            //     $"Sector {generatedSector.Id} has {generatedSector.SchemeAreas.Count} areas " +
            //     $"and {sectorAreaConnectivity.Nodes.Count} connectivity nodes");
            // if (generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count) {
            //     Debug.LogWarning("generatedSector.SchemeAreas.Count != sectorAreaConnectivity.Nodes.Count");
            // }

            SetAllocatedRequestedZones(
                generatedSector,
                requestedSector.Zones,
                requestedSector.ZoneGroups,
                sectorAreaConnectivity,
                requestedSector.AreaAllocationRequest.Id);
        }
    }

    private SectorRequest FindRequestedSector(int generatedSectorId) =>
        context
            .Request
            .RequestedSectors
            .FirstOrDefault(x => GenerationData.GetGeneratedSectorId(x) == generatedSectorId);

    private void SetAllocatedRequestedZones(
        GeneratedSectorInfo generatedSector,
        List<ZoneRequest> zones,
        List<AllocatableAreaGroup> zoneGroups,
        Graph<int> sectorAreaConnectivity,
        int sectorRequestId)
    {
        var areaAllocator = CreateAllocator(generatedSector);

        var generatedAreaIdsByAllocationRequestId = areaAllocator.AllocateAreas(
            zones.Select(x => x.AreaAllocationRequest),
            zoneGroups,
            sectorAreaConnectivity
        ).GeneratedAreaIdsByAllocationRequestId;

        // Initialize dictionary for later use
        requestedZonesBySchemeAreaId = generatedAreaIdsByAllocationRequestId.ToDictionary(
            x => x.Value, // Area Id
            x => zones.First(y => y.AreaAllocationRequest.Id == x.Key) // ZoneRequest
        );

        GenerationData.GeneratedAreaIdsByAllocationRequestIdBySectorRequestId.Add(
            sectorRequestId,
            generatedAreaIdsByAllocationRequestId
        );
    }

    private AreaAllocator CreateAllocator(GeneratedSectorInfo generatedSector) {
        var areaFilter = new ZoneAreaAllocatorFilter(generatedSector.SchemeAreas);
        return new AreaAllocator(areaFilter);
    }

    #endregion

    #region Apply
    private void ApplyAllocationToGeneratedZones() {
        foreach (var generatedSector in GenerationData.GeneratedSectors) {
            SetGeneratedZonesForSector(generatedSector);
        }
    }

    private ZoneRequest GetZoneRequest(int schemeAreaId) {
        requestedZonesBySchemeAreaId.TryGetValue(schemeAreaId, out var zoneRequest);
        return zoneRequest;
    }

    private void SetGeneratedZonesForSector(GeneratedSectorInfo generatedSector)
    {
        generatedSector.Zones = new();

        foreach (var schemeArea in generatedSector.SchemeAreas) {
            var zone = new GeneratedZone() {
                SchemeAreaId = schemeArea.Id,
                ZoneFillingInfo = GetZoneRequest(schemeArea.Id)?.ZoneFillingInfo
                    ?? zoneSelector.GenerateZoneFillingInfo(generatedSector, context.Request)
            };
            generatedSector.Zones.Add(zone);
        }

        HandleDebugColorForGeneratedZones(generatedSector);
    }

    #endregion

    #region Debug
    private void HandleDebugColorForGeneratedZones(GeneratedSectorInfo generatedSector)
    {
        if (addDebugColorForGeneratedZones) {
            foreach (var zone in generatedSector.Zones) {
                ComplexGenerator.AddDebugAreaColor(
                    zone.SchemeAreaId,
                    DebugColorMarkUtils.GetNextColor());
            }   
        }
    }

    private void LogWarning() {
        if (GenerationData.GeneratedSectors.Count != GenerationData.AreaPossibleConnectivityBySectorId.Count)
        {
            Debug.LogWarning(
                $"Generated sectors: {GenerationData.GeneratedSectors.Count}. " +
                $"Connectivity: {GenerationData.AreaPossibleConnectivityBySectorId.Count}");   
        }
    }
    #endregion
}
