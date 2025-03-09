using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IZoneFillingStrategyProvider))]
public class ZoneFilling : GenerationStage
{
    [SerializeField]
    private IZoneFillingStrategyProvider zoneFillingStrategyProvider;

    private void Awake() {
        zoneFillingStrategyProvider = GetComponent<IZoneFillingStrategyProvider>();
    }

    public override void RunStage()
    {
        FillSectorZones();
    }

    private void FillSectorZones() {
        foreach (var sector in context.GenerationData.GeneratedSectors) {
            FillZones(sector);
        }
    }

    private void FillZones(GeneratedSectorInfo generatedSector)
    {
        foreach (var generatedZone in generatedSector.Zones) {
            var strategies = zoneFillingStrategyProvider.GetZoneFillingStrategies(
                generatedZone,
                generatedSector);
            ApplyStrategies(generatedZone, strategies);
        }
    }

    private void ApplyStrategies(
        GeneratedZone generatedZone,
        List<IZoneFillingStrategy> strategies)
    {
        foreach (var strategy in strategies) {
            strategy.Apply(generatedZone, context);
        }
    }
}
