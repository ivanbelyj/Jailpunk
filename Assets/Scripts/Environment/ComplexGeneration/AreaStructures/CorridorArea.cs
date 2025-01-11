using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CorridorArea : ITraversableArea
{
    private bool isDebug;
    public List<CorridorAreaPart> Parts { get; set; }
    public int Breadth { get; set; }

    public CorridorArea(int breadth, bool isDebug = false) {
        Breadth = breadth;
        Parts = new List<CorridorAreaPart>();
        this.isDebug = isDebug;
    }

    public CorridorArea(Vector2Int pos,
        int length, int breadth, bool isVertical, bool isDebug = false)
            : this(breadth, isDebug) {
        Parts.Add(new CorridorAreaPart(pos, length, breadth, isVertical));
    }

    public CorridorArea(
        Vector2Int pos,
        int xLength,
        int yLength,
        int breadth,
        bool isDebug = false)
            : this(breadth, isDebug) {

        bool shouldAddVertCorridor;
        if (xLength == 0) {
            AddNewPart(pos, yLength, true);
            // Already added
            shouldAddVertCorridor = false;
        } else {
            // Horizontal corridor part is added first if it's needed ( != 0)
            AddNewPart(pos, xLength, false);
            shouldAddVertCorridor = yLength != 0;
        }
        if (shouldAddVertCorridor) {
            AddNewPartFromPrevious(yLength, true);
        }

        // Debug.Log("Create corridor space: " + Parts[0] + " " + Parts[1]);
    }

    public void AddNewPart(Vector2Int pos, int length, bool isVert) {
        Parts.Add(new CorridorAreaPart(pos, length, Breadth, isVert));
        if (isDebug)
            Debug.Log($"Added new corridor part: {Parts[^1]}");
    }

    public void AddNewPartFromPrevious(int length, bool isVert) {
        if (Parts == null || Parts.Count == 0)
            throw new InvalidOperationException();

        var newPartPos = Parts[^1].EndPos;
        if (isVert) {
            if (length < 0) {
                newPartPos.y += Breadth;
                length += -Breadth;
            } else {
                length += 1;
            }
        }
        if (isDebug) {
            ComplexGenerator.AddDebugMarkToScheme(newPartPos, Color.magenta);
        }

        AddNewPart(newPartPos, length, isVert);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        int number = 1;
        foreach (var part in Parts) {
            sb.Append($"part {number++}: {part}; ");
        }
        return sb.ToString();
    }

    public void Traverse(ITraversableArea.TraverseDelegate callback)
    {
        for (int i = 0; i < Parts.Count; i++) {
            var part = Parts[i];
            var nextPart = (i + 1) < Parts.Count ? Parts[i + 1] : null;
            var prevPart = (i - 1) >= 0 ? Parts[i - 1] : null;
            TraverseRectUtils.TraverseRect(part.RectWithPositiveSize, (data) => {
                bool isBorder = data.isBorder;
                int x = data.x;
                int y = data.y;

                bool isWall = GeometryUtils.IsOnRectBorder(part.Rect, x, y);
                
                // The task is to determine the free passage
                // between corridor parts
                //     ==....==
                //     ==....==
                // ======....==
                // ....==....==  <--
                // ============
                // 
                //     ^
                //     |
                if (// There can be the end passage of the
                    // previous corridor
                    prevPart != null && prevPart.IsOnStraightPassage(x, y))
                {
                    
                    isWall = false;
                }
                    
                callback(x, y, isWall); 
            });
        }
    }
}
