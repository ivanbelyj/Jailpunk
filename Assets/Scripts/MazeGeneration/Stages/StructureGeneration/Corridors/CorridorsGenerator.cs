using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorsGenerator
{
    /// <summary>
    /// Creates corridors connecting two rooms
    /// </summary>
    public  List<CorridorArea> CreateCorridors(RectArea room1, RectArea room2,
        int corridorBreadth = 3) {
        List<CorridorArea> corridors = new List<CorridorArea>();

        Vector2Int point1 = room1.GetInnerRandomPoint(corridorBreadth);
        Vector2Int point2 = room2.GetInnerRandomPoint(corridorBreadth);

        // int xDiff = point2.x - point1.x;
        // int yDiff = point2.y - point1.y;
        int xDiff = point2.x - point1.x;  // 30
        int yDiff = point2.y - point1.y;  // -25
        // var boundingRect = GeometryUtils.GetBoundingRect(
        //     new [] { room1, room2}
        //     .Select(rectSpace => rectSpace.Rect)
        //     .ToArray());
        

        // Debug.Log($"room1 rect: {room1.Rect}; room2 rect: {room2.Rect}");
        // Debug.Log($"point in room 1: {point1}; point in room 2: {point2}");
        // Debug.Log("xDiff: " + xDiff + "; yDiff: " + yDiff);

        // void AddNewCorridor(int x, int y, int length, bool isVert) {
        //     corridors.Add(new CorridorSpace(new Vector2Int(x, y),
        //         length, corridorBreadth, isVert));
        // }
        void AddNewCorridor(int x, int y, int xLength, int yLength) {
            var newCorridor = new CorridorArea(new Vector2Int(x, y),
                xLength, yLength, corridorBreadth);
            // Debug.Log("created new corridor. " + newCorridor);
            corridors.Add(newCorridor);
        }

        AddNewCorridor(point1.x, point1.y, xDiff, yDiff);

        // // 2|1
        // if (xDiff < 0) {
            
        //     if (yDiff < 0) {
        //         // 2|.
        //         // .|1
        //         if (Random.value < 0.5f) {
        //             // t .
        //             // | .
        //             // f-.

        //             // AddNewCorridor(point2.x, point1.y, xDiff, false);
        //             // AddNewCorridor(point2.x, point2.y, yDiff, true);

        //             // corridors.Add(new RectInt(point2.x, point1.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point2.x, point2.y, 1, Mathf.Abs(yDiff)));

        //         } else {
        //             // AddNewCorridor(point2.x, point2.y, xDiff, false);
        //             // AddNewCorridor(point2.x, point2.y, yDiff, true);

        //             // corridors.Add(new RectInt(point2.x, point2.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point1.x, point2.y, 1, Mathf.Abs(yDiff)));
        //         }
        //     } else if (yDiff > 0) {
        //         // .|1
        //         // 2|.
        //         if (Random.value < 0.5f) {
        //             // AddNewCorridor(point2.x, point1.y, xDiff, false);
        //             // AddNewCorridor(point2.x, point1.y, yDiff, true);

        //             // corridors.Add(new RectInt(point2.x, point1.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point2.x, point1.y, 1, Mathf.Abs(yDiff)));
        //         } else {
        //             // AddNewCorridor(point2.x, point2.y, xDiff, false);
        //             // AddNewCorridor(point1.x, point1.y, yDiff, true);

        //             // corridors.Add(new RectInt(point2.x, point2.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point1.x, point1.y, 1, Mathf.Abs(yDiff)));
        //         }
        //     } else  // if (h == 0)
        //     {
        //         // 2|1
        //         // .|.
        //         // AddNewCorridor(point2.x, point2.y, xDiff, false);

        //         // corridors.Add(new RectInt(point2.x, point2.y, Mathf.Abs(xDiff), 1));
        //     }
        // } else if (xDiff > 0) {
        //     // 1|2

        //     if (yDiff < 0) {
        //         // .|2
        //         // 1|.
        //         if (Random.value < 0.5f) {
        //             // AddNewCorridor(point1.x, point2.y, xDiff, false);
        //             // AddNewCorridor(point1.x, point2.y, yDiff, true);


        //             // corridors.Add(new RectInt(point1.x, point2.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point1.x, point2.y, 1, Mathf.Abs(yDiff)));
        //         } else {
        //             // AddNewCorridor(point1.x, point1.y, xDiff, false);
        //             // AddNewCorridor(point2.x, point2.y, yDiff, true);

        //             // corridors.Add(new RectInt(point1.x, point1.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point2.x, point2.y, 1, Mathf.Abs(yDiff)));
        //         }
        //     } else if (yDiff > 0) {
        //         // 1|.
        //         // .|2
        //         if (Random.value < 0.5f) {
        //             // AddNewCorridor(point1.x, point1.y, xDiff, false);
        //             // AddNewCorridor(point2.x, point1.y, yDiff, true);

        //             // corridors.Add(new RectInt(point1.x, point1.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point2.x, point1.y, 1, Mathf.Abs(yDiff)));
        //         } else {
        //             // AddNewCorridor(point1.x, point2.y, xDiff, false);
        //             // AddNewCorridor(point1.x, point1.y, yDiff, true);

        //             // corridors.Add(new RectInt(point1.x, point2.y, Mathf.Abs(xDiff), 1));
        //             // corridors.Add(new RectInt(point1.x, point1.y, 1, Mathf.Abs(yDiff)));
        //         }
        //     } else  // if (h == 0)
        //     {
        //         // 1|2
        //         // .|.
        //         // AddNewCorridor(point1.x, point1.y, xDiff, false);

        //         // corridors.Add(new RectInt(point1.x, point1.y, Mathf.Abs(xDiff), 1));
        //     }
        // } else  // if (w == 0)
        // {
        //     if (yDiff < 0) {
        //         // 2|.
        //         // 1|.
        //         // AddNewCorridor(point2.x, point2.y, yDiff, true);

        //         // corridors.Add(new RectInt(point2.x, point2.y, 1, Mathf.Abs(yDiff)));
        //     } else if (yDiff > 0) {
        //         // 1|.
        //         // 2|.
        //         // AddNewCorridor(point1.x, point1.y, yDiff, true);

        //         // corridors.Add(new RectInt(point1.x, point1.y, 1, Mathf.Abs(yDiff)));
        //     }
        // }
        
        return corridors;
    }
}
