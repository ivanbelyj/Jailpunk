using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationRequest {
    [SerializeField]
    private GenerationParameters parameters;

    [SerializeField]
    private ComplexGenerationSchema complexGenerationSchema;

    private List<SectorRequest> requestedSectors;

    private List<AllocatableAreaGroup> sectorGroups;

    public GenerationParameters Parameters {
        get => parameters;
        set => parameters = value;
    }

    public ComplexGenerationSchema ComplexGenerationSchema {
        get => complexGenerationSchema;
        set => complexGenerationSchema = value;
    }

    /// <summary>
    /// Sectors defined by high-level logic, such as plot necessity
    /// </summary>
    public List<SectorRequest> RequestedSectors {
        get => requestedSectors;
        set => requestedSectors = value;
    }
    
    public List<AllocatableAreaGroup> SectorGroups {
        get => sectorGroups;
        set => sectorGroups = value;
    }
}
