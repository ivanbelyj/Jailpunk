using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationRequest {
    [SerializeField]
    private GenerationParameters parameters;

    [SerializeField]
    private List<RequestedSectorInfo> requestedSectors;

    [SerializeField]
    private List<SectorGroup> sectorGroups;

    public GenerationParameters Parameters => parameters;

    /// <summary>
    /// Sectors defined by high-level logic, such as plot necessity
    /// </summary>
    public List<RequestedSectorInfo> RequestedSectors => requestedSectors;

    public List<SectorGroup> SectorGroups => sectorGroups;
}
