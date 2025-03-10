using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneFillingStrategyBuilder
{
    public List<IZoneFillingStrategy> BuildStrategiesFromSchema(
        SectorZoneGenerationSchema sectorZoneGenerationSchema,
        SectorGenerationSchema sectorGenerationSchema) {
        return sectorZoneGenerationSchema
            .zoneFillingSchemas
            .Concat(sectorGenerationSchema.commonSectorZoneFillingSchemas)
            .OrderBy(x => x.order)
            .Select(BuildStrategy)
            .ToList();
    }

    private IZoneFillingStrategy BuildStrategy(ZoneFillingLayerSchema zoneFillingSchema) {
        return zoneFillingSchema.fillingType switch {
            ZoneFillingType.Empty => new EmptyZoneFillingStrategy(),
            ZoneFillingType.Solid => new SolidZoneFillingStrategy(
                (zone) => zoneFillingSchema.traverseRectFilter,
                zoneFillingSchema.layerTargetOptions,
                zoneFillingSchema.mapObjectSchemaAddress
            ),
            ZoneFillingType.MazeWithRooms => new MazeWithRoomsFillingStrategy(
                (zone) => zoneFillingSchema.traverseRectFilter,
                zoneFillingSchema.layerTargetOptions,
                zoneFillingSchema.mapObjectSchemaAddress),
            ZoneFillingType.Test => new TestZoneFillingStrategy((zone) => zoneFillingSchema.traverseRectFilter),
            _ => throw new NotImplementedException()
        };
    }
}