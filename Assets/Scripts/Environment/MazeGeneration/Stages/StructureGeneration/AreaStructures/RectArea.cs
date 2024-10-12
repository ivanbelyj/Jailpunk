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
    public enum Side {
        Left,
        Right,
        Top,
        Bottom
    }

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

    public Vector2Int GetSideRandomPoint(int offsetFromFirstCorner,
        Side side, int? offsetFromSecondCorner = null) {
        if (offsetFromSecondCorner == null) {
            offsetFromSecondCorner = offsetFromFirstCorner;
        }

        bool isOnVertSide;
        bool isFirstBorder;
        switch (side) {
            case Side.Left:
                isOnVertSide = true;
                isFirstBorder = true;
                break;
            case Side.Right:
                isOnVertSide = true;
                isFirstBorder = false;
                break;
            case Side.Top:
                isOnVertSide = false;
                isFirstBorder = true;
                break;
            case Side.Bottom:
                isOnVertSide = false;
                isFirstBorder = false;
                break;
            default:
                throw new System.ArgumentException("Unknown RectArea Side");
        }

        return GetSideRandomPoint(offsetFromFirstCorner, offsetFromSecondCorner,
            isOnVertSide, isFirstBorder);
    }

    /// <summary>
    /// Some point placed on the RectArea border with the defined offset
    /// from the corner
    /// </summary>
    public Vector2Int GetSideRandomPoint(int offsetFromFirstCorner,
        int? offsetFromSecondCorner = null,
        bool? isOnVertSide = null, bool? isFirstBorder = null) {
        int x, y;
        if (offsetFromSecondCorner == null) {
            offsetFromSecondCorner = offsetFromFirstCorner;
        }

        int RandomValBySide(int posMin, int posMax) {
            return Random.Range(posMin + offsetFromFirstCorner,
                posMax - 1 - offsetFromSecondCorner.Value);
        }

        bool isVert = isOnVertSide == null ?
            Random.value < 0.5f : isOnVertSide.Value;
        bool isFirst = isFirstBorder == null ?
            Random.value < 0.5f : isFirstBorder.Value;
        
        if (isVert) {
            y = RandomValBySide(Rect.y, Rect.yMax);
            x = isFirst ? Rect.xMin : Rect.xMax - 1;
        } else {
            x = RandomValBySide(Rect.x, Rect.xMax);
            y = isFirst ? Rect.yMin : Rect.yMax - 1;
        }

        return new Vector2Int(x, y);
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

    public override string ToString()
    {
        return Rect.ToString();
    }
}
