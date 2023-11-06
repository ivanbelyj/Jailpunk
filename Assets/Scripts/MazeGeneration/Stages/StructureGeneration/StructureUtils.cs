using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureUtils : MonoBehaviour
{
    private static void TraverseRect(RectInt rect, Action<int, int, bool> callback) {
        int roomRightLimit = rect.x + rect.width - 1;
        int roomBottomLimit = rect.y + rect.height - 1;
        for (int y = rect.y; y <= roomBottomLimit; y++) {
            for (int x = rect.x; x <= roomRightLimit; x++) {
                bool isBorder = y == rect.y || y == roomBottomLimit;
                if (!isBorder)
                    isBorder = x == rect.x || x == roomRightLimit;
                callback(x, y, isBorder);
            }
        }
    }

    public static void ApplyNewSector(MazeScheme scheme, RectSpace room,
        int sectorId) {
        RectInt rect = room.Rect;
        SchemeSector sector = new SchemeSector() {
            SectorId = sectorId
        };
        TraverseRect(rect, (x, y, isBorder) => {
            SchemeTile tile = scheme.GetTileByPos(x, y);
            tile.SectorId = sectorId;
            tile.TileType = isBorder ?
                TileType.LoadBearingWall : TileType.Floor;
        });
    }

    public static void ApplyCorridor(MazeScheme scheme, CorridorSpace corridor) {
        RectInt rect = corridor.Rect;
        TraverseRect(rect, (x, y, isBorder) => {
            SchemeTile tile = scheme.GetTileByPos(x, y);
            if (tile.TileType == TileType.NoSpace) {
                tile.TileType = TileType.LoadBearingWall;
            }
        });
    }
}
