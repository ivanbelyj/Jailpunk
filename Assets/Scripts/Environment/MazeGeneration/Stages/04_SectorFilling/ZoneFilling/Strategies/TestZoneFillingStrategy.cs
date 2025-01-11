using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestZoneFillingStrategy : TraverseZoneFillingStrategy
{
    public override void ApplyPosition(
        SchemeArea area,
        SchemeTile tile,
        TraverseRectData data)
    {
        
        if (true)//tile.TileType == TileType.Floor)
        {
            MazeGenerator.AddDebugMarkToScheme(
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
}
