using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Intermediate generation representation of rectangle space.
/// In the next generation stages it would be a sector or something else.
/// Used on structure generation stage
/// </summary>
[System.Serializable]
public class RectArea : IRectArea, ITraverseable
{
    [SerializeField]
    private RectInt rect;
    public RectInt Rect {
        get => rect;
        set => rect = value;
    }
    public RectArea(int x, int y, int width, int height) {
        Rect = new RectInt(x, y, width, height);
    }
    public RectArea(RectInt rect) {
        Rect = rect;
    }
    
    public Vector2Int GetInnerRandomPoint(int padding) {
        Vector2Int point = new Vector2Int(
            Random.Range(Rect.xMin + 1 + padding, Rect.xMax - 2 - padding),
            Random.Range(Rect.yMin + 1 + padding, Rect.yMax - 2 - padding));
        return point;
    }

    public void Traverse(ITraverseable.TraversalDelegate callback) {
        int roomRightLimit = rect.x + rect.width - 1;
        int roomBottomLimit = rect.y + rect.height - 1;
        for (int y = rect.y; y <= roomBottomLimit; y++) {
            for (int x = rect.x; x <= roomRightLimit; x++) {
                bool isWall = y == rect.y || y == roomBottomLimit;
                if (!isWall)
                    isWall = x == rect.x || x == roomRightLimit;
                callback(x, y, isWall);
            }
        }
    }
}
