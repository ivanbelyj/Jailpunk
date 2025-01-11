using System;
using UnityEngine;

[Serializable]
public class TraverseRectFilter
{
    [Header("Distance to edge")]
    public bool useMinDistanceToEdge;
    public int minDistanceToEdge = 3;

    public bool useMaxDistanceToEdge;
    public int maxDistanceToEdge = 5;

    [Header("Distance to center")]
    public bool useMinDistanceToCenter;
    public int minDistanceToCenter = 3;

    public bool useMaxDistanceToCenter;
    public int maxDistanceToCenter = 5;
}