using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IsometricUtils
{
    public static Vector3 CartesianToIsometric(Vector3 cartPos) {
        Vector3 res = new Vector3();
        res.x = cartPos.x - cartPos.y;
        res.y = (cartPos.x + cartPos.y) / 2;
        return res;
    }

    public static Vector3 RotateCartesian(Vector3 cartPos) {
        Vector3 res = new Vector3();
        res.x = cartPos.y + cartPos.x;
        res.y = cartPos.y - cartPos.x;
        return res;
    }

    public static Vector3 IsometricToCartesian(Vector3 isoPos) {
        Vector3 res = new Vector3();
        res.x = (2 * isoPos.y + isoPos.x) / 2;
        res.y = (2 * isoPos.y - isoPos.x) / 2;
        return res;
    }

    public static Vector2 MazeObjectOrientationToVector2(
        IsometricDirection orient) {
        Vector2 dir = MazeObjectOrientationToCartesianVector2(orient);
        return CartesianToIsometric(dir);
    }

    private static Vector2 MazeObjectOrientationToCartesianVector2(
        IsometricDirection orient) {
        Vector2 dir = orient switch {
            IsometricDirection.North => Vector2.up,
            IsometricDirection.East => Vector2.right,
            IsometricDirection.South => Vector2.down,
            IsometricDirection.West => Vector2.left,

            IsometricDirection.NorthEast => Rotated(Vector2.up, -45f),
            IsometricDirection.NorthWest => Rotated(Vector2.up, 45f),
            IsometricDirection.SouthEast => Rotated(Vector2.down, 45f),
            IsometricDirection.SouthWest => Rotated(Vector2.down, -45f),
            
            _ => throw new System.ArgumentException("Unknown MazeObjectOrientation")
        };
        return dir;
    }

    private static Vector2 Rotated(Vector2 vector, float angleZ) {
        return Quaternion.Euler(0, 0, angleZ) * vector;
    }

    public static Vector2 MazeObjectOrientationToRotated90Vector2(
        IsometricDirection orient) {
        Vector2 dir = Rotated(MazeObjectOrientationToCartesianVector2(orient), 90f);
        return CartesianToIsometric(dir);
    }
}
