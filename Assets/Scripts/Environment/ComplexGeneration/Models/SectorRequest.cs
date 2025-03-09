using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sector requirements defined on higher level by the plot necessity
/// </summary>
[System.Serializable]
public class SectorRequest
{
    [SerializeField]
    private List<AllocatableAreaGroup> zoneGroups = new();

    [SerializeField]
    private List<ZoneRequest> zones = new();

    [SerializeField]
    private AreaAllocationRequest areaAllocationRequest;

    [SerializeField]
    private SectorPlanningInfo sectorPlanningInfo;

    public List<AllocatableAreaGroup> ZoneGroups {
        get => zoneGroups;
        set => zoneGroups = value;
    }
    public AreaAllocationRequest AreaAllocationRequest {
        get => areaAllocationRequest;
        set => areaAllocationRequest = value;
    }

    public List<ZoneRequest> Zones {
        get => zones;
        set => zones = value;
    }

    public SectorPlanningInfo SectorPlanningInfo {
        get => sectorPlanningInfo;
        set => sectorPlanningInfo = value;
    }
}
