using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureUtils
{
    /// <summary>
    /// Traversing a Rect is not the same as the traversing ITraverseable:
    /// TraverseRect provides data about a border, but not necessarily a wall.
    /// For example, borders can be determined from CorridorAreaPart,
    /// but walls - only from CorridorArea (because of the corner passages)
    /// </summary>
    public delegate void TraverseRectDelegate(int x, int y, bool isBorder);
    
    public static void TraverseRect(
        RectInt rect,
        TraverseRectDelegate callback) {
        for (int y = rect.y; y <= rect.yMax - 1; y++) {
            for (int x = rect.x; x <= rect.xMax - 1; x++) {
                bool isBorder = y == rect.y || y == rect.yMax - 1
                    || x == rect.x || x == rect.xMax - 1;
                callback(x, y, isBorder);
            }
        }
    }

    // public static void ApplyCorridor(MazeScheme scheme, CorridorArea corridor) {
    //     foreach (var part in corridor.Parts) {
    //         RectInt rect = part.RectWithPositiveSize;
    //         TraverseRect(rect, (x, y, isBorder) => {
    //             if (x < 0 || y < 0 ||
    //                 x >= scheme.MapSize.x || y >= scheme.MapSize.y) {
    //                 Debug.LogError("Incorrect corridor rect. " +
    //                     "Map size exceeded");
    //                 return;
    //             }
    //             SchemeTile tile = scheme.GetTileByPos(x, y);
    //             if (tile.TileType == TileType.NoSpace) {
    //                 tile.TileType = TileType.LoadBearingWall;
    //             }
    //         });
    //     }
    // }
}
