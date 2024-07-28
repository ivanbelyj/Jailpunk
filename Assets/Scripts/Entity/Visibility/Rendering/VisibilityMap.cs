using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityMap
{
    public float[,] Map { get; private set; }
    private const int OutOfScreenPadding = 2;

    private Vector2Int screenSizeInCells;

    /// <summary>
    /// Setter may invalidate VisibilityMap (it should be updated then)
    /// </summary>
    public Vector2Int ScreenSizeInCells {
        get => screenSizeInCells;

        set {
            screenSizeInCells = value;

            int newSizeY = screenSizeInCells.y + OutOfScreenPadding * 2;
            int newSizeX = screenSizeInCells.x + OutOfScreenPadding * 2;

            if (Map == null
                || newSizeY != Map.GetLength(0)
                || newSizeX != Map.GetLength(1)) {
                Map = new float[
                    newSizeY,
                    newSizeX
                ];
            }
        }
    }
    
    public Vector2Int MinCell { get; set; }

    public float this[int row, int col] {
        get => Map[row, col];
        set => Map[row, col] = value;
    }

    public int SizeX => Map.GetLength(1);
    public int SizeY => Map.GetLength(0);

    /// <param name="process">Accepts row, col in VisibilityMap
    /// and according coordinates in the grid</param>
    public void TraverseVisibilityMap(
        Action<int, int, int, int> process) {
        for (
            int x = MinCell.x - OutOfScreenPadding, col = 0;
            x < MinCell.x + screenSizeInCells.x + OutOfScreenPadding;
            x++, col++) {
            for (
                int y = MinCell.y - OutOfScreenPadding, row = 0;
                y < MinCell.y + screenSizeInCells.y + OutOfScreenPadding;
                y++, row++) {
                process(row, col, x, y);
            }
        }
    }
}
