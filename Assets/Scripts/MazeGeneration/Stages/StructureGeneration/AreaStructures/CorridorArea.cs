using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CorridorArea : ITraverseable
{
    public List<CorridorAreaPart> Parts { get; set; }
    public int Breadth { get; set; }

    public CorridorArea(int breadth) {
        Breadth = breadth;
        Parts = new List<CorridorAreaPart>();
    }

    public CorridorArea(Vector2Int pos,
        int length, int breadth, bool isVertical) : this(breadth) {
        Parts.Add(new CorridorAreaPart(pos, length, breadth, isVertical));
    }
    public CorridorArea(Vector2Int pos,
        int xLength, int yLength, int breadth) : this(breadth) {
        // AddNewCorridor(point2.x, point1.y, xDiff, false);
        // AddNewCorridor(point2.x, point2.y, yDiff, true);

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
    }

    public void AddNewPartFromPrevious(int length, bool isVert) {
        if (Parts == null || Parts.Count == 0)
            throw new InvalidOperationException();
        var newPartPos = Parts[Parts.Count - 1].EndPos;
        if (isVert && length < 0) {
            newPartPos.y += Breadth;
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

    public void Traverse(ITraverseable.TraversalDelegate callback)
    {
        for (int i = 0; i < Parts.Count; i++) {
            var part = Parts[i];
            StructureUtils.TraverseRect(part.Rect, (x, y, isBorder) => {
                bool isWall = GeometryUtils.IsOnRectBorder(part.Rect, x, y);
                // if (part.IsOnStraightPassage(x, y))
                //     isWall = false;
                // Todo:
                callback(x, y, isWall); 
            });
        }
    }
}
