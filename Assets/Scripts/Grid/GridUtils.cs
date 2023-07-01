using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    private const string unknownDirectionMessage = "Unknown IsometricDirection";

    private static Vector2 GridDirectionToCartesianVector2(
        GridDirection orient) {
        Vector2 dir = orient switch {
            GridDirection.North => Vector2.up,
            GridDirection.East => Vector2.right,
            GridDirection.South => Vector2.down,
            GridDirection.West => Vector2.left,

            GridDirection.NorthEast => Rotated(Vector2.up, -45f),
            GridDirection.NorthWest => Rotated(Vector2.up, 45f),
            GridDirection.SouthEast => Rotated(Vector2.down, 45f),
            GridDirection.SouthWest => Rotated(Vector2.down, -45f),
            
            _ => throw new System.ArgumentException(unknownDirectionMessage)
        };
        return dir;
    }

    public static Vector3 RotateCartesian(Vector3 cartPos) {
        Vector3 res = new Vector3();
        res.x = cartPos.y + cartPos.x;
        res.y = cartPos.y - cartPos.x;
        return res;
    }

    private static Vector2 Rotated(Vector2 vector, float angleZ) {
        return Quaternion.Euler(0, 0, angleZ) * vector;
    }

    /// <summary>
    /// Converts IsometricDirection to Vector2Int, where x is
    /// horizontal and y is vertical coordinate
    /// </summary>
    public static Vector2Int GridDirectionToVector2Int(GridDirection dir) {
        var north = new Vector2Int(0, 1);
        var east = new Vector2Int(1, 0);
        var south = new Vector2Int(0, -1);
        var west = new Vector2Int(-1, 0);
        return dir switch {
            GridDirection.North => north,
            GridDirection.East => east,
            GridDirection.South => south,
            GridDirection.West => west,

            GridDirection.NorthEast => north + east,
            GridDirection.NorthWest => north + west,
            GridDirection.SouthEast => south + east,
            GridDirection.SouthWest => south + west,
            _ => throw new System.ArgumentException(unknownDirectionMessage)
        };
    }
}
