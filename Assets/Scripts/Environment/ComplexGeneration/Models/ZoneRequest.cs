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
    private ZoneFillingInfo zoneFillingInfo;

    public AreaAllocationRequest AreaAllocationRequest 
    {
        get => areaAllocationRequest;
        set => areaAllocationRequest = value;
    }

    public ZoneFillingInfo ZoneFillingInfo {
        get => zoneFillingInfo;
        set => zoneFillingInfo = value;
    }
}
