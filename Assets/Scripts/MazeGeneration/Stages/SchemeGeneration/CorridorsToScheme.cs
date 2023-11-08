using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorsToScheme : GenerationStage
{
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        MazeScheme scheme = context.MazeData.Scheme;
        foreach (CorridorArea corridor in context.Corridors) {
            corridor.Traverse((x, y, isWall) => {
                SchemeTile tile = scheme.GetTileByPos(x, y);
                if (tile.TileType == TileType.NoSpace) {
                    tile.TileType = isWall ? TileType.LoadBearingWall
                    : TileType.Floor;
                }
            });
        }
        return context;
    }
}
