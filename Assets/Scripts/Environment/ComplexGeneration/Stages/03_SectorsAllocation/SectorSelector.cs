using System.Linq;
using UnityEngine;

public enum SectorSelectionType {
    WeightedRandom
}

public class SectorSelector : MonoBehaviour, ISectorSelector
{
    [SerializeField]
    private SectorSelectionType sectorSelectionType;

    private AssetManager assetManager;

    private void Awake() {
        assetManager = FindAnyObjectByType<AssetManager>();
    }
    
    public SectorPlanningInfo GenerateSectorPlanningInfo(
        GeneratedSectorInfo sectorInfo,
        GenerationRequest generationRequest)
    {
        var sectorGenerationSchema = sectorSelectionType switch {
            SectorSelectionType.WeightedRandom => GetWeightedRandomSchema(generationRequest),
            _ => throw new System.NotImplementedException()
        };

        return new() {
            SectorGenerationSchemaId = sectorGenerationSchema.sectorGenerationSchemaId
        };
    }

    private SectorGenerationSchema GetWeightedRandomSchema(GenerationRequest generationRequest)
    {
        var sectorVariants = generationRequest.ComplexGenerationSchema.sectorVariants;
        return RandomUtils.GetRandomWeighted(
            sectorVariants, 
            sectorVariants.Select(x => x.weight).ToList()).sectorGenerationSchema;
    }
}
