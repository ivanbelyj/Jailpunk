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

    public static bool IsOnRect(RectInt rect, int x, int y) {
        return x >= rect.xMin && x <= rect.xMax - 1 &&
            y >= rect.yMin && y <= rect.yMax - 1;
    }

    /// <summary>
    /// Checks if segments a and b have intersection of minimum breadth
    /// </summary>
    public static bool IntersectWithBreadth(int a1, int a2, int b1, int b2,
        int minBreadth) {
        if (a2 - a1 + 1 < minBreadth || b2 - b1 + 1 < minBreadth)
            return false;

        bool res;
        // 1st case
        // ------
        //    ------
        // 012345678
        if (b1 >= a1)
            res = a2 - b1 + 1 >= minBreadth;

        // 2nd case
        //    ------
        // ------
        // 012345678
        else {
            res = b2 - a1 + 1 >= minBreadth;
            // 5 - 3 + 1 == 3
        }
        return res;
    }

    public static (int, int) GetIntersectionSegment(int a1, int a2,
        int b1, int b2) {
        return (a1 > b1 ? a1 : b1, a2 < b2 ? a2 : b2);
    }
}
