using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsGenerator
{
    /// <summary>
    /// Creates corridors connecting two rooms
    /// </summary>
    public List<CorridorSpace> CreateCorridors(RectSpace leftRoom, RectSpace rightRoom) {
        List<RectInt> corridors = new List<RectInt>();

        Vector2Int pointLeft = leftRoom.GetInnerRandomPoint();
        Vector2Int pointRight = rightRoom.GetInnerRandomPoint();

        int w = pointRight.x - pointLeft.x;
        int h = pointRight.y - pointLeft.y;

        if (w < 0) {
            if (h < 0) {
                if (Random.value < 0.5f) {
                    corridors.Add(new RectInt(pointRight.x, pointLeft.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
                } else {
                    corridors.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointLeft.x, pointRight.y, 1, Mathf.Abs(h)));
                }
            } else if (h > 0) {
                if (Random.value < 0.5f) {
                    corridors.Add(new RectInt(pointRight.x, pointLeft.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointRight.x, pointLeft.y, 1, Mathf.Abs(h)));
                } else {
                    corridors.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
                }
            } else  // if (h == 0)
            {
                corridors.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
            }
        } else if (w > 0) {
            if (h < 0) {
                if (Random.value < 0.5f) {
                    corridors.Add(new RectInt(pointLeft.x, pointRight.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointLeft.x, pointRight.y, 1, Mathf.Abs(h)));
                } else {
                    corridors.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
                }
            } else if (h > 0) {
                if (Random.value < 0.5f) {
                    corridors.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointRight.x, pointLeft.y, 1, Mathf.Abs(h)));
                } else {
                    corridors.Add(new RectInt(pointLeft.x, pointRight.y, Mathf.Abs(w), 1));
                    corridors.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
                }
            } else  // if (h == 0)
            {
                corridors.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
            }
        } else  // if (w == 0)
        {
            if (h < 0) {
                corridors.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
            } else if (h > 0) {
                corridors.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
            }
        }
        
        return corridors.Select(rect => new CorridorSpace() {
            Rect = rect,
            IsHorizontal = true // Todo
        }).ToList();
    }
}
