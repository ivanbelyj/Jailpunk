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
        var schema = assetManager.SectorGenerationSchemas.GetAssetById(
            sector.SectorPlanningInfo.SectorGenerationSchemaId
        );
        return sectorPlanningStrategyBuilder.BuildStrategyFromSchema(schema);
    }

    public List<IZoneFillingStrategy> GetZoneFillingStrategies(GeneratedZone generatedZone)
    {
        var schema = assetManager.SectorZoneGenerationSchemas.GetAssetById(
            generatedZone.ZoneFillingInfo.ZoneGenerationSchemaId
        );
        return zoneFillingStrategyBuilder.BuildStrategiesFromSchema(schema);
    }
}
