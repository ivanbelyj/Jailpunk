using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightingUtils
{
    public static float CalculateLighting(
        Vector3 origin,
        Vector3 target,
        float innerRadius,
        float outerRadius)
    {
        float distance = Vector3.Distance(origin, target);

        if (distance <= innerRadius)
        {
            return 1f;
        }

        if (distance >= outerRadius)
        {
            return 0f;
        }

        // Normalized distance
        return 1f - (distance - innerRadius) / (outerRadius - innerRadius);
    }
}
