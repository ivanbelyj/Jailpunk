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

    public static Vector3 IsometricToCartesian(Vector3 isoPos) {
        Vector3 res = new Vector3();
        res.x = (2 * isoPos.y + isoPos.x) / 2;
        res.y = (2 * isoPos.y - isoPos.x) / 2;
        return res;
    }
}
