using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCostHeuristicUtils
{
    public static float ManhattanEstimate(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Math.Abs(posA.x - posB.x) + Math.Abs(posA.y - posB.y);
    }

    public static float EuclideanEstimate(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Vector3.Distance(posA, posB);
    }
}
