using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathUtils 
{
    public static bool IsPathClear(Vector2 origin, Vector2 destination, float pathWidth)
    {
        Vector2 direction = destination - origin;
        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            origin,
            pathWidth / 2,
            direction,
            distance, 1 << LayerMask.NameToLayer("VisionObstacle"));
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null)
                return false;
        }

        return true;
    }
}
