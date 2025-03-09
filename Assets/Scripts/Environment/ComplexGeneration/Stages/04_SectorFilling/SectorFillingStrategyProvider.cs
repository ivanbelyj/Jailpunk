using System.Collections.Generic;
using UnityEngine;

public class SectorFillingStrategyProvider :
    MonoBehaviour,
    ISectorPlanningStrategyProvider,
    IZoneFillingStrategyProvider
{
    private AssetManager assetManager;

    private SectorPlanningStrategyBuilder sectorPlanningStrategyBuilder;
    private ZoneFillingStrategyBuilder zoneFillingStrategyBuilder;

    private void Awake() {
        sectorPlanningStrategyBuilder = new(IdGenerator.Instance);
        zoneFillingStrategyBuilder = new();

        assetManager = FindAnyObjectByType<AssetManager>();
    }

    public ISectorPlanningStrategy GetSectorPlanningStrategy(GeneratedSectorInfo sector)
    {
        var schema = GetSectorGenerationSchema(sector);
        return sectorPlanningStrategyBuilder.BuildStrategyFromSchema(schema);
    }

    public List<IZoneFillingStrategy> GetZoneFillingStrategies(
        GeneratedZone generatedZone,
        GeneratedSectorInfo generatedSector)
    {
        var schema = assetManager.SectorZoneGenerationSchemas.GetAssetById(
            generatedZone.ZoneFillingInfo.ZoneGenerationSchemaId
        );
        var sectorGenerationSchema = GetSectorGenerationSchema(generatedSector);
        return zoneFillingStrategyBuilder.BuildStrategiesFromSchema(schema, sectorGenerationSchema);
    }

    private SectorGenerationSchema GetSectorGenerationSchema(GeneratedSectorInfo generatedSector) {
        return assetManager.SectorGenerationSchemas.GetAssetById(
            generatedSector.SectorPlanningInfo.SectorGenerationSchemaId
        );
    }
}
