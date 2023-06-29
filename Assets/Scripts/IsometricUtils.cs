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
        MazeObjectOrientation orient) {
        Vector2 dir = orient switch {
            MazeObjectOrientation.North => Vector2.up,
            MazeObjectOrientation.East => Vector2.right,
            MazeObjectOrientation.South => Vector2.down,
            MazeObjectOrientation.West => Vector2.left,
            _ => throw new System.ArgumentException("Unknown MazeObjectOrientation")
        };
        return CartesianToIsometric(dir);
    }
}
