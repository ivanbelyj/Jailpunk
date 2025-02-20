using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridDirectionUtils
{
    public const int AngleTopRight = 45;
    public const int AngleBottomRight = 135;
    public const int AngleBottomLeft = 225;
    public const int AngleTopLeft = 315;

    private const string UnknownDirectionMessage = "Unknown GridDirection";

    public static Vector3 RotateCartesian(Vector3 cartPos) {
        Vector3 res = new Vector3();
        res.x = cartPos.y + cartPos.x;
        res.y = cartPos.y - cartPos.x;
        return res;
    }

    public static Vector2 Rotated(Vector2 vector, float angleZ) {
        return Quaternion.Euler(0, 0, angleZ) * vector;
    }

    /// <summary>
    /// Converts GridDirection to Vector2Int, where x is
    /// horizontal and y is vertical coordinate
    /// </summary>
    public static Vector2Int GridDirectionToVectorInt(GridDirection dir) {
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
            _ => throw new System.ArgumentException(UnknownDirectionMessage)
        };
    }

    public static Vector2 GridDirectionToCartesianVector(
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
            
            _ => throw new System.ArgumentException(UnknownDirectionMessage)
        };
        return dir;
    }

    /// <summary>
    /// 0 according to North, 45 according to East, etc.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static GridDirection AngleToGridDirection(float angle) {
        return angle switch {
            >= 0 and <= 22 => GridDirection.North,
            > 22 and <= 67 => GridDirection.NorthEast,
            > 67 and <= 112 => GridDirection.East,
            > 112 and <= 157 => GridDirection.SouthEast,
            > 157 and <= 202 => GridDirection.South,
            > 202 and <= 247 => GridDirection.SouthWest,
            > 247 and <= 292 => GridDirection.West,
            > 292 and <= 337 => GridDirection.NorthWest,
            > 337 and <= 360 => GridDirection.North,
            _ => throw new ArgumentOutOfRangeException(
                $"Invalid {nameof(angle)} value: {angle}. " +
                "Angle must be between 0 and 359")
        };
    }

    public static GridDirection VectorToDirection(Vector2 vector) {
        var angle = (-Vector2.SignedAngle(Vector2.up, vector) + 360) % 360;
        return AngleToGridDirection(angle);
    }

    /// <summary>
    /// There are only 4 animation angles are supported, so different angles
    /// should be adjusted
    /// </summary>
    public static int AdjustToAngleSupportedByAnimation(int angle) {
        return angle switch {
            >= 0 and < 35 => 0, // 45
            >= 35 and <= 145 => 90,
            > 145 and < 215 => 180, // 135, 225
            >= 215 and <= 325 => 270,
            > 325 and <= 360 => 0, // 315
            _ => throw new ArgumentOutOfRangeException(
                $"Invalid {nameof(angle)} value: {angle}. " +
                "Angle must be in 0 (inclusive), 360 (inclusive)")
        };
    }


    public static int GridDirectionToAngle(GridDirection dir) {
        return dir switch {
            GridDirection.North => 000,
            GridDirection.East => 090,
            GridDirection.South => 180,
            GridDirection.West => 270,

            GridDirection.NorthEast => AngleTopRight,
            GridDirection.NorthWest => AngleTopLeft,
            GridDirection.SouthEast => AngleBottomRight,
            GridDirection.SouthWest => AngleBottomLeft,
            _ => throw new ArgumentException("")
        };
    }

    /// <summary>
    /// Only 4 animation angles are supported, but there are 8 grid directions
    /// </summary>
    public static int GridDirectionToAnimationAngle(GridDirection dir) {
        return AdjustToAngleSupportedByAnimation(GridDirectionToAngle(dir));
    }

    public static bool IsVertical(GridDirection gridDirection) {
        var angle = GridDirectionToAngle(gridDirection);
        return angle > AngleTopLeft
            || angle < AngleTopRight
            || angle > AngleBottomRight && angle < AngleBottomLeft;
    }
}
