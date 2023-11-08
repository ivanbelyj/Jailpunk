using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Intermediate representation of the space on the basis of which
/// the corridors will be created.
/// Used on structure generation stage.
/// Corridor area part cannot be traversed without parent corridor area
/// (because of detecting of walls)
/// </summary>
public class CorridorAreaPart : IRectArea
{
    private Vector2Int Pos { get; set; }
    
    public int Length { get; private set; }
    // public int PositiveLength { get => Mathf.Abs(Length); }
    public int Breadth { get; private set; }
    public bool IsVertical { get; private set; }

    public CorridorAreaPart(Vector2Int pos, int length, int breadth, bool isVertical) {
        Pos = pos;
        Length = length;
        Breadth = breadth;
        IsVertical = isVertical;
    }

    public RectInt Rect {
        get {
            Vector2Int size = new Vector2Int(
                IsVertical ? Breadth : Length,
                IsVertical ? Length : Breadth);
            return new RectInt(Pos, size);
        }
    }

    /// <summary>
    /// The point on the center of the corridor start edge
    /// </summary>
    // public Vector2Int StartPoint {
    //     get {
    //         return PointOnTheCenterOfTheCorridorEdge(Pos, false);
    //     }
    // }

    public Vector2Int EndPos {
        get {
            Vector2Int point = IsVertical ?
                new Vector2Int(Pos.x,
                    Pos.y + Length) :
                new Vector2Int(Pos.x + Length - 1,
                    Pos.y);
            return point;
        }
    }

    public Vector2Int GetCorridorEdgeOffset(
        bool isEndPoint) {
        int x, y;
        int lengthSign = (int)Mathf.Sign(Length);
        int halfBreadth = Breadth / 2;
        int signedHalfBreadth = lengthSign * halfBreadth
            * (isEndPoint ? -1 : 1);
        if (IsVertical) {
            x = signedHalfBreadth;
            y = halfBreadth;
        } else {
            x = halfBreadth;
            y = signedHalfBreadth;
        }
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Position of the top left corner of the corridor part
    /// (position adjusted to the positive rect)
    /// </summary>
    public Vector2Int PosAdjusted {
        get {
            Vector2Int adjustedPos = Pos;

            if (Length < 0) {
                if (IsVertical) {
                    adjustedPos.y += Length;
                } else {
                    adjustedPos.x += Length;
                }
            }
            
            return adjustedPos;
        }
    }

    /// <summary>
    /// Corridor space part rectangle with positive size
    /// and ajusted position for accordance to the positive size
    /// </summary>
    public RectInt RectWithPositiveSize {
        get {
            int lengthAbs = Mathf.Abs(Length);
            Vector2Int positiveSize = new Vector2Int(
                IsVertical ? Breadth : lengthAbs,
                IsVertical ? lengthAbs : Breadth);

            //  = Pos + new Vector2Int(
            //     IsVertical  ? 0 : (Length < 0 ? 0 : Mathf.Abs(Length)),
            //     !IsVertical ? 0 : (Length < 0 ? 0 : Mathf.Abs(Length))
            // );
            
            return new RectInt(PosAdjusted, positiveSize); 
        }
    }

    /// <summary>
    /// End point of the corridor part adjusted to the positive rect
    /// </summary>
    public Vector2Int EndPosAdjusted {
        get {
            var adjustedPos = PosAdjusted;
            int lengthSign = (int)Mathf.Sign(Length);
            return IsVertical ?
                new Vector2Int(adjustedPos.x,
                    adjustedPos.y + Length - lengthSign) :
                new Vector2Int(adjustedPos.x + Length - lengthSign,
                    adjustedPos.y);
        }
    }
    public override string ToString()
    {
        return $"{RectWithPositiveSize}";
    }

    public bool IsOnStraightPassage(int x, int y) {
        bool isOnPassageLine = IsVertical ?
            y == PosAdjusted.y || y == EndPosAdjusted.y:
            x == PosAdjusted.x || x == EndPosAdjusted.x;

        bool IsOnAxe(int val, int axeCrossInterval) {
            return val > axeCrossInterval &&
                val < axeCrossInterval + Breadth - 1;
        }

        bool isOnPassageAxe = IsVertical ?
            IsOnAxe(x, PosAdjusted.x) :
            IsOnAxe(y, PosAdjusted.y);
        return isOnPassageLine && isOnPassageAxe;
    }

}
