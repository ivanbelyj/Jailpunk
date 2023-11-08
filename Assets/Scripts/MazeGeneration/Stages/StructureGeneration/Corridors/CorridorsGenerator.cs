using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

using Side = RectArea.Side;

public class CorridorsGenerator
{
    private bool isDebug;
    public CorridorsGenerator(bool isDebug = false) {
        this.isDebug = isDebug;
    }

    public (Vector2Int, Vector2Int)? GetTwoConnectablePoints(RectArea room1,
        RectArea room2, int corridorBreadth) {

        int xDiff = room1.Rect.x - room2.Rect.x;
        int yDiff = room1.Rect.y - room2.Rect.y;

        var (leftRoom, rightRoom) = xDiff < 0 ?
            (room1, room2) : (room2, room1);
        var (topRoom, bottomRoom) = yDiff < 0 ?
            (room1, room2) : (room2, room1);

        if (isDebug)
            Debug.Log($"Trying to connect rooms. left room: {leftRoom}; right room: {rightRoom}");
        
        Vector2Int point1;
        Vector2Int point2; 

        // If can get create a straight corridor from left to right
        int leftYMin = leftRoom.Rect.yMin;
        int leftYMax = leftRoom.Rect.yMax - 1;
        int rightYMin = rightRoom.Rect.yMin;
        int rightYMax = rightRoom.Rect.yMax - 1;
        if (GeometryUtils.IntersectWithBreadth(
            leftYMin, leftYMax,
            rightYMin, rightYMax,
            corridorBreadth)) {
            
            (int offset1, int offset2) = GeometryUtils.GetIntersectionSegment(
                leftYMin, leftYMax, rightYMin, rightYMax);
            point1 = leftRoom.GetSideRandomPoint(
                offset1,
                Side.Right,
                offset2);
            point2 = point1;
            point2.x = rightRoom.Rect.xMin;

            if (isDebug)
                Debug.Log($"Straight left-right corridor from {point1} to {point2}");
            return (point1, point2);
        }

        // If can create a straight corridor from top to bottom
        int topXMin = topRoom.Rect.xMin;
        int topXMax = topRoom.Rect.xMax;
        int bottomXMin = bottomRoom.Rect.xMin;
        int bottomXMax = bottomRoom.Rect.xMax;
        if (GeometryUtils.IntersectWithBreadth(
            topXMin, topXMax,
            bottomXMin, bottomXMax,
            corridorBreadth)) {
            (int offset1, int offset2) = GeometryUtils.GetIntersectionSegment(
                topXMin, topXMax, bottomXMin, bottomXMax);
            point1 = topRoom.GetSideRandomPoint(
                offset1,
                Side.Right,
                offset2);
            point2 = point1;
            point2.y = bottomRoom.Rect.yMin;

            if (isDebug)
                Debug.Log($"Straight top-bottom corridor from {point1} to {point2}");
            return (point1, point2);
        }

        // Not straight corridors (with two parts)
        
        if (leftRoom.Rect.xMax - 1 + corridorBreadth < rightRoom.Rect.xMin) {
            // Connectable variant 1
            point1 = leftRoom.GetSideRandomPoint(corridorBreadth, Side.Right);
            point2 = rightRoom.GetSideRandomPoint(corridorBreadth,
                leftYMin < rightYMin ? Side.Top : Side.Bottom);
        } else {
            return null;
        }

        if (isDebug)
            Debug.Log($"Corridor from {point1} to {point2}");
        return (point1, point2);
    }
    
    /// <summary>
    /// Creates corridors connecting two rooms.
    /// Returns corridor areas and two points used for corridors generation
    /// </summary>
    public List<CorridorArea>
        CreateCorridors(RectArea room1, RectArea room2,
        int corridorBreadth = 3) {
        List<CorridorArea> corridors = new List<CorridorArea>();

        var connPoints = GetTwoConnectablePoints(room1, room2, corridorBreadth);
        if (connPoints == null)
            return null;

        Vector2Int point1 = connPoints.Value.Item1;
        Vector2Int point2 = connPoints.Value.Item2;

        if (isDebug) {
            MazeGenerator.AddDebugMarkToScheme(point1, Color.green);
            MazeGenerator.AddDebugMarkToScheme(point2, Color.red);
        }


        int xDiff = point2.x - point1.x;
        int yDiff = point2.y - point1.y;
        
        void AddNewCorridor(int x, int y, int xLength, int yLength) {
            var newCorridor = new CorridorArea(new Vector2Int(x, y),
                xLength, yLength, corridorBreadth, isDebug);
            corridors.Add(newCorridor);
        }

        AddNewCorridor(point1.x, point1.y, xDiff, yDiff);
        
        return corridors;
    }
}
