using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Traversing a Rect is not the same as the traversing ITraverseable:
/// TraverseRect provides data about a border, but not necessarily a wall.
/// For example, borders can be determined from CorridorAreaPart,
/// but walls - only from CorridorArea (because of the corner passages)
/// </summary>
public delegate void TraverseRectDelegate(TraverseRectData traverseData);

public static class TraverseRectUtils
{
    public static void TraverseRect(
        RectInt rect,
        TraverseRectDelegate callback,
        int offset = 0)
    {
        if (offset != 0) {
            var offsetVector = new Vector2Int(offset, offset);
            rect = new RectInt(
                rect.position + offsetVector,
                rect.size - 2 * offsetVector);
        }
        for (int y = rect.y; y <= rect.yMax - 1; y++) {
            for (int x = rect.x; x <= rect.xMax - 1; x++) {
                bool isBorder = y == rect.y || y == rect.yMax - 1
                    || x == rect.x || x == rect.xMax - 1;

                int distanceToEdge = GetDistanceToEdge(rect, x, y);

                Vector2 relativePosition = new Vector2(
                    (float)(x - rect.x) / rect.width,
                    (float)(y - rect.y) / rect.height
                );

                float distanceToCenter = Mathf.RoundToInt(Vector2.Distance(
                    new Vector2(x, y),
                    rect.center));

                callback(new TraverseRectData() {
                    x = x,
                    y = y,
                    isBorder = isBorder,
                    distanceToEdge = distanceToEdge,
                    relativePosition = relativePosition,
                    distanceToCenter = distanceToCenter,
                });
            }
        }
    }

    private static int GetDistanceToEdge(RectInt rect, int x, int y)
        => Mathf.Min(
            x - rect.x,              // Left
            rect.xMax - 1 - x,       // Right
            y - rect.y,              // Up
            rect.yMax - 1 - y        // Down
        );
}
