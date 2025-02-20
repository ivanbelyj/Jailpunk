using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationRequest {
    [SerializeField]
    private GenerationParameters parameters;

    private List<SectorInfo> requestedSectors;

    private List<AllocatableAreaGroup> sectorGroups;

    public GenerationParameters Parameters {
        get => parameters;
        set => parameters = value;
    }

    /// <summary>
    /// Sectors defined by high-level logic, such as plot necessity
    /// </summary>
    public List<SectorInfo> RequestedSectors {
        get => requestedSectors;
        set => requestedSectors = value;
    }
    
    public List<AllocatableAreaGroup> SectorGroups {
        get => sectorGroups;
        set => sectorGroups = value;
    }
}
