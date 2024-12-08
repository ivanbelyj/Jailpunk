using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreasBoundary
{
    public int AreaA { get; set; }
    public int AreaB { get; set; }
    public HashSet<Vector2Int> BoundaryTilesA { get; set; } = new();
    public HashSet<Vector2Int> BoundaryTilesB { get; set; } = new();
}

public class ConnectivityResult {
    public Graph<int> ConnectivityGraph { get; set; }
    public List<AreasBoundary> Boundaries { get; set; }
}

// Warning: algorithms were not intentionally optimized.
// The efficiency of this code may be improved

public class ConnectivityProcessor
{
    /// <summary>
    /// Can be considered as the minimum length of the passage between areas
    /// </summary>
    private const int MinBoundaryLength = 3;

    /// <summary>
    /// Can be considered as wall length of the passage between areas
    /// </summary>
    private const int MinBoundaryBorder = 1;

    private Dictionary<(int areaA, int areaB), AreasBoundary> boundariesByProcessedPair;
    
    public ConnectivityResult ProcessConnectivity(
        Func<int, int, int?> getAreaId,
        Vector2Int size,
        Vector2Int? startPos = null)
    {
        startPos ??= Vector2Int.zero;

        boundariesByProcessedPair = new();

        Traverse(getAreaId, size, startPos.Value);

        return new ConnectivityResult() {
            Boundaries = boundariesByProcessedPair.Values.ToList(),
            ConnectivityGraph = ToGraph(boundariesByProcessedPair.Keys),
        };
    }

    private Graph<int> ToGraph(IEnumerable<(int areaA, int areaB)> areaPairs) {
        var res = new Graph<int>();
        var addedNodesByAreaId = new Dictionary<int, Node<int>>();
        foreach (var pair in areaPairs) {
            Node<int> EnsureNodeAdded(int areaId) {
                if (!addedNodesByAreaId.TryGetValue(areaId, out Node<int> node)) {
                    node = new Node<int>(areaId);
                    addedNodesByAreaId.Add(areaId, node);
                }
                return node;
            }

            var nodeA = EnsureNodeAdded(pair.areaA);
            var nodeB = EnsureNodeAdded(pair.areaB);

            res.AddLink(nodeA, nodeB);
        }
        return res;
    }

    private void Traverse(
        Func<int, int, int?> getAreaId,
        Vector2Int size,
        Vector2Int startPos)
    {
        for (int y = startPos.y; y < size.y; y++)
        {
            for (int x = startPos.x; x < size.x; x++)
            {
                ProcessTile(getAreaId, x, y);
            }
        }
    }

    private void ProcessTile(Func<int, int, int?> getAreaId, int x, int y)
    {
        var currentTile = getAreaId(x, y);
        if (currentTile == null) {
            return;
        }

        // Right
        ProcessDirection(getAreaId, x, y, false); 

        // Down
        ProcessDirection(getAreaId, x, y, true); 
    }

    private void ProcessDirection(
        Func<int, int, int?> getAreaId,
        int startPosX,
        int startPosY,
        bool isVertical)
    {
        var shouldProcessDirection = ProcessDirectionCore(
            getAreaId, 
            startPosX,
            startPosY,
            isVertical,
            out var areasPair,
            out var boundaryTilesA,
            out var boundaryTilesB);
            
        if (!shouldProcessDirection) {
            return;
        }

        if (!boundariesByProcessedPair.ContainsKey(areasPair)) {
            boundariesByProcessedPair.Add(
                areasPair,
                new() {
                    AreaA = areasPair.areaA,
                    AreaB = areasPair.areaB,
                    BoundaryTilesA = new(),
                    BoundaryTilesB = new()
                });
        }
        var boundary = boundariesByProcessedPair[areasPair];
        AddMissingTiles(boundary.BoundaryTilesA, boundaryTilesA);
        AddMissingTiles(boundary.BoundaryTilesB, boundaryTilesB);

        void AddMissingTiles(HashSet<Vector2Int> boundaryTiles, List<Vector2Int> newTiles) {
            foreach (var newTile in newTiles) {
                if (!boundaryTiles.Contains(newTile)) {
                    boundaryTiles.Add(newTile);
                }
            }
        }
    }

    /// <summary>
    /// True if direction processing should be continued
    /// </summary>
    private bool ProcessDirectionCore(
        Func<int, int, int?> getAreaId,
        int startPosX,
        int startPosY,
        bool isVertical,
        out (int areaA, int areaB) areasPair,
        out List<Vector2Int> boundaryTilesA,
        out List<Vector2Int> boundaryTilesB) {
        boundaryTilesA = null;
        boundaryTilesB = null;
        areasPair = default;

        var directionTiles = GetDirectionTiles(startPosX, startPosY, isVertical);

        bool areFirstAreasSet = false;
        int areaA = -1;
        int areaB = -1;
        foreach (var (tilePosA, tilePosB) in directionTiles) {
            var currentAreaA = getAreaId(tilePosA.x, tilePosA.y);
            var currentAreaB = getAreaId(tilePosB.x, tilePosB.y);

            if (currentAreaA == null || currentAreaB == null || currentAreaA == currentAreaB
                || areFirstAreasSet
                    && (currentAreaA.Value != areaA || currentAreaB.Value != areaB)) {
                // The current direction is not a boundary between two different areas
                return false;
            }

            if (!areFirstAreasSet) {
                areaA = currentAreaA.Value;
                areaB = currentAreaB.Value;
                areFirstAreasSet = true;
            }
        }
        
        areasPair = (Mathf.Min(areaA, areaB), Mathf.Max(areaA, areaB));
        

        boundaryTilesA = new();
        boundaryTilesB = new();
        
        for (int i = MinBoundaryBorder; i < directionTiles.Count - MinBoundaryBorder; i++) {
            boundaryTilesA.Add(directionTiles[i].tilePosA);
            boundaryTilesB.Add(directionTiles[i].tilePosB);
        }

        return true;
    }

    private static List<(Vector2Int tilePosA, Vector2Int tilePosB)> GetDirectionTiles(
        int startPosX,
        int startPosY,
        bool isVertical) {
        var res = new List<(Vector2Int tilePosA, Vector2Int tilePosB)>();
        var start = isVertical ? startPosY : startPosX;
        for (
            int i = start;
            i < start + MinBoundaryLength;
            i++) {
            int mainX = isVertical ? startPosX : i;
            int mainY = isVertical ? i : startPosY;
            res.Add(
                (
                    tilePosA: new Vector2Int(mainX, mainY),
                    tilePosB: new Vector2Int(
                        mainX + (isVertical ? 1 : 0),
                        mainY + (isVertical ? 0 : 1))
                ));
        }
        return res;
    }
}
