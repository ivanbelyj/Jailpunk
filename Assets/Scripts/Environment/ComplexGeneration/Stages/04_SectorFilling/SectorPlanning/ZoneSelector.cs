using System.Linq;
using UnityEngine;

public enum ZoneSelectionType {
    WeightedRandom
}

public class ZoneSelector : MonoBehaviour, IZoneSelector
{
    [SerializeField]
    private ZoneSelectionType zoneSelectionType;

    private AssetManager assetManager;
    
    private void Awake()
    {
        assetManager = FindAnyObjectByType<AssetManager>();
    }

    public ZoneFillingInfo GenerateZoneFillingInfo(
        GeneratedSectorInfo generatedSectorInfo,
        GenerationRequest generationRequest)
    {
        var sectorSchema = assetManager.SectorGenerationSchemas.GetAssetById(
            generatedSectorInfo.SectorPlanningInfo.SectorGenerationSchemaId);
        
        var zoneGenerationSchema = zoneSelectionType switch {
            ZoneSelectionType.WeightedRandom => GetWeightedRandomSchema(
                sectorSchema,
                generationRequest),
            _ => throw new System.NotImplementedException()
        };

        return new() {
            ZoneGenerationSchemaId = zoneGenerationSchema.sectorZoneGenerationSchemaId,
        };
    }

    private SectorZoneGenerationSchema GetWeightedRandomSchema(
        SectorGenerationSchema sectorGenerationSchema,
        GenerationRequest generationRequest)
    {
        var zoneVariants = sectorGenerationSchema.zoneVariants;
        if (zoneVariants == null) {
            Debug.LogError("zone variants are null");
        }
        var result = RandomUtils.GetRandomWeighted(
            zoneVariants,
            zoneVariants.Select(x => x.weight).ToList()).sectorZoneGenerationSchema;
        if (result == null) {
            Debug.LogError("sectorZoneGenerationSchema is null");
        }
        return result;
    }
}
