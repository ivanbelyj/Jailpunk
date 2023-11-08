using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryUtils
{
    public static RectInt GetBoundingRect(params RectInt[] rects)
    {
        if (rects.Length == 0)
        {
            return new RectInt();
        }

        int minX = rects[0].xMin;
        int minY = rects[0].yMin;
        int maxX = rects[0].xMax;
        int maxY = rects[0].yMax;

        foreach (var rect in rects)
        {
            if (rect.xMin < minX) minX = rect.xMin;
            if (rect.yMin < minY) minY = rect.yMin;
            if (rect.xMax > maxX) maxX = rect.xMax;
            if (rect.yMax > maxY) maxY = rect.yMax;
        }

        return new RectInt(minX, minY, maxX - minX, maxY - minY);
    }

    public static bool IsOnRectBorder(RectInt rect, int x, int y) {
        return x == rect.xMax - 1 || x == rect.xMin ||
            y == rect.yMin || y == rect.yMax - 1;
    }

    public static bool IsOnRectButNotBorder(RectInt rect, int x, int y) {
        return x > rect.xMin && x < rect.xMax - 1 &&
            y > rect.yMin && y < rect.yMax - 1;
    }
}
