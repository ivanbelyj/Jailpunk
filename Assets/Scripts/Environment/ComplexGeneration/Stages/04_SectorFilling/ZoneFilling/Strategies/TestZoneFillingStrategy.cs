using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestZoneFillingStrategy : TraverseZoneFillingStrategy
{
    public TestZoneFillingStrategy(Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory) : base(traverseRectFilterFactory)
    {
    }

    public override void ApplyPosition(TraverseZoneFillingStrategyPositionContext context)
    {
        var data = context.TraverseData;
        ComplexGenerator.AddDebugMarkToScheme(
            new Vector2Int(data.x, data.y),
            data.distanceToCenter switch {
                0 => Color.black,
                1 => Color.magenta,
                2 => Color.green,
                3 => Color.cyan,
                4 => Color.blue,
                _ => Color.magenta
            });
    }
}
