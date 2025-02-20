using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sector requirements defined on higher level by the plot necessity
/// </summary>
[System.Serializable]
public class SectorRequest
{
    [SerializeField]
    private int sectorGroupId;

    [SerializeField]
    private List<AllocatableAreaGroup> zoneGroups = new();

    [SerializeField]
    private AreaAllocationRequest areaAllocationRequest;

    public int SectorGroupId {
        get => sectorGroupId;
        set => sectorGroupId = value;
    }
    public List<AllocatableAreaGroup> ZoneGroups {
        get => zoneGroups;
        set => zoneGroups = value;
    }
    public AreaAllocationRequest AreaAllocationRequest {
        get => areaAllocationRequest;
        set => areaAllocationRequest = value;
    }
}

[System.Serializable]
public class SectorInfo
{
    [SerializeField]
    private SectorRequest sectorRequest;

    [SerializeField]
    private List<ZoneInfo> zones = new();

    /// <summary>
    /// Zones requested for the sector
    /// </summary>
    public List<ZoneInfo> Zones {
        get => zones;
        set => zones = value;
    }

    public SectorRequest SectorRequest {
        get => sectorRequest;
        set => sectorRequest = value;
    }

    /// <summary>
    /// May be set during the generation
    /// </summary>
    // public int? GeneratedSectorId { get; private set; }
}
