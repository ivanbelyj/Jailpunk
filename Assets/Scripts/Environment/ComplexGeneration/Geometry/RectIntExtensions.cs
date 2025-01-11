using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectIntExtensions
{
    public static float GetAspectRatio(this RectInt rect) =>
        rect.width > rect.height
            ? rect.width / (float)rect.height
            : rect.height / (float)rect.width;
    
    public static bool AreConstraintsSatisfied(
        this RectInt rect,
        RectConstraints constraints)
    {
        var aspectRatio = rect.GetAspectRatio();
        var geometricMean = Mathf.Sqrt(rect.width * rect.height);
        
        return (!constraints.useMinAspectRatio || aspectRatio >= constraints.minAspectRatio)
            && (!constraints.useMaxAspectRatio || aspectRatio <= constraints.maxAspectRatio)
            && (!constraints.useMinGeometricMean || geometricMean >= constraints.minGeometricMean)
            && (!constraints.useMaxGeometricMean || aspectRatio <= constraints.maxGeometricMean);
    }
}
