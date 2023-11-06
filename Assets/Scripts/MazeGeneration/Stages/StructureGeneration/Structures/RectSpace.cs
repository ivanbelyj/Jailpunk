using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Intermediate generation representation of rectangle space.
/// In the next generation stages it would be a sector or something else.
/// Used on structure generation stage
/// </summary>
public class RectSpace
{
    public RectInt Rect { get; set; }
    public RectSpace(int x, int y, int width, int height) {
        Rect = new RectInt(x, y, width, height);
    }
    public RectSpace(RectInt rect) {
        Rect = rect;
    }
    
    public Vector2Int GetInnerRandomPoint() {
        Vector2Int point = new Vector2Int(
            Random.Range(Rect.xMin + 1, Rect.xMax - 2),
            Random.Range(Rect.yMin + 1, Rect.yMax - 2));
        return point;
    }
}
