using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationRequest {
    [SerializeField]
    private GenerationParameters parameters;

    [SerializeField]
    private List<SectorInfo> requestedSectors;

    [SerializeField]
    private List<AllocatableAreaGroup> sectorGroups;

    public GenerationParameters Parameters => parameters;

    /// <summary>
    /// Sectors defined by high-level logic, such as plot necessity
    /// </summary>
    public List<SectorInfo> RequestedSectors => requestedSectors;

    public List<AllocatableAreaGroup> SectorGroups => sectorGroups;
}
