using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorsToScheme : GenerationStage
{
    [Header("Debug")]
    public bool applyOverScheme = false;
    public override void RunStage()
    {
        ComplexScheme scheme = context.ComplexData.Scheme;
        int mapXMax = scheme.MapSize.x - 1;
        int mapYMax = scheme.MapSize.y - 1;
        foreach (CorridorArea corridor in GenerationData.Corridors) {
            var corridorSectorId = idGenerator.NewSectorId();
            corridor.Traverse((x, y, isWall) => {
                if (x < 0 || y < 0 || x > mapXMax || y > mapYMax) {
                    // Debug.LogWarning("Corridor outside of the map");
                    return;
                }

                SchemeTile tile = scheme.GetTileByPos(x, y);
                if (applyOverScheme || tile.TileType == TileType.NoSpace) {
                    tile.TileType = isWall
                        ? TileType.LoadBearingWall
                        : TileType.Floor;
                    tile.SectorId = corridorSectorId;
                }
            });
        }
    }
}
