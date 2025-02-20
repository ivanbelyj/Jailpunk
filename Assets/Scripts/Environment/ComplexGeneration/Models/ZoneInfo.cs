using UnityEngine;

/// <summary>
/// Sector zone is a space with dedicated responsibility, functionally segregated
/// from other. Defined by the one of sector responsibilities, generated
/// on the level, higher than generation stages
/// </summary>
[System.Serializable]
public class ZoneRequest
{
    [SerializeField]
    private AreaAllocationRequest areaAllocationRequest;

    [SerializeField]
    private int zoneGroupId;

    public AreaAllocationRequest AreaAllocationRequest 
    {
        get => areaAllocationRequest;
        set => areaAllocationRequest = value;
    }
    public int ZoneGroupId
    {
        get => zoneGroupId;
        set => zoneGroupId = value;
    }
}

[System.Serializable]
public class ZoneInfo
{
    [SerializeField]
    private ZoneRequest zoneRequest;

    public ZoneRequest ZoneRequest
    {
        get => zoneRequest;
        set => zoneRequest = value;
    }
}
