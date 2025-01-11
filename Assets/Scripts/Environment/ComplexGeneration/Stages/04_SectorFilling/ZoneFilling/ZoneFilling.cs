using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneFilling : GenerationStage
{
    // Todo: for test it's here now
    [SerializeField]
    private TraverseRectFilter traverseRectFilter;

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
            var strategies = SelectStrategies(generatedZone);
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

    private List<IZoneFillingStrategy> SelectStrategies(GeneratedZone generatedZone) {
        return new List<IZoneFillingStrategy> {
            // new TestZoneFillingStrategy()
            new MazeWithRoomsFillingStrategy((zone) => traverseRectFilter)
        };
    }
}
