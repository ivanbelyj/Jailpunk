using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneFillingStrategyBuilder
{
    public List<IZoneFillingStrategy> BuildStrategiesFromSchema(SectorZoneGenerationSchema sectorZoneGenerationSchema) {
        return sectorZoneGenerationSchema.zoneFillingSchemas.Select(BuildStrategy).ToList();
    }

    private IZoneFillingStrategy BuildStrategy(ZoneFillingLayerSchema zoneFillingSchema) {
        return zoneFillingSchema.fillingType switch {
            ZoneFillingType.Empty => new EmptyZoneFillingStrategy(),
            ZoneFillingType.MazeWithRooms => new MazeWithRoomsFillingStrategy(
                (zone) => zoneFillingSchema.traverseRectFilter),
            ZoneFillingType.Test => new TestZoneFillingStrategy(),
            _ => throw new NotImplementedException()
        };
    }
}