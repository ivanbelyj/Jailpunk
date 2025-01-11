using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectConstraints
{
    public bool useMinGeometricMean = true;
    public float minGeometricMean = 6f;
    
    public bool useMaxGeometricMean = true;
    public float maxGeometricMean = 12f;

    public bool useMinAspectRatio = false;
    public float minAspectRatio = 1f;

    public bool useMaxAspectRatio = true;
    public float maxAspectRatio = 2f;
}
