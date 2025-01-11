using UnityEngine;

public struct TraverseRectData {
    public bool isBorder;
    public int x;
    public int y;
    public int distanceToEdge;
    public float distanceToCenter;
    public Vector2 relativePosition;

    public bool IsFilterSatisfied(TraverseRectFilter filter) =>
        (!filter.useMinDistanceToEdge || distanceToEdge >= filter.minDistanceToEdge) &&
        (!filter.useMaxDistanceToEdge || distanceToEdge <= filter.maxDistanceToEdge) &&
        (!filter.useMinDistanceToCenter || distanceToCenter >= filter.minDistanceToCenter) &&
        (!filter.useMaxDistanceToCenter || distanceToCenter <= filter.maxDistanceToCenter);
}