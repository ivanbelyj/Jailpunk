using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityProcessor : MonoBehaviour
{
    [SerializeField]
    private Transform observer;
    public Transform Observer { 
        get => observer;
        set => observer = value;
     }

    private GridManager gridManager;
    private GridManager GridManager {
        get {
            if (gridManager == null) {
                gridManager = FindObjectOfType<GridManager>();
            }
            return gridManager;
        }
    }
    private GridLightSource[] lightSources;

    public VisibilityMap VisibilityMap { get; private set; }

    public void UpdateLightSources() {
        lightSources = FindObjectsOfType<GridLightSource>();
    }

    public void UpdateVisibility() {
        UpdateLightSources(); // TODO: more efficient support of changing sources

        UpdateVisibilityMap();

        VisibilityMap.TraverseVisibilityMap((row, col, x, y) => {
            VisibilityMap[row, col] = CalculateVisibility(x, y);
        });

        float[,] map2 = new float[VisibilityMap.SizeY, VisibilityMap.SizeX];
        VisibilityMap.TraverseVisibilityMap((row, col, x, y) => {
            bool isObstacle = gridManager.IsObstacle(new(x, y, 0));
            if (isObstacle && HasNeighborGreaterThanZero(row, col, false)) {
                map2[row, col] = CalculateVisibility(x, y, true);
            } else {
                map2[row, col] = VisibilityMap[row, col];
            }
        });
        VisibilityMap.TraverseVisibilityMap((row, col, x, y) => {
            VisibilityMap[row, col] = map2[row, col];
        });
    }

    private bool HasNeighborGreaterThanZero(int row, int col, bool checkDiagonals)
    {
        int rowsCount = VisibilityMap.Map.GetLength(0);
        int colsCount = VisibilityMap.Map.GetLength(1);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                if (!checkDiagonals && i != 0 && j != 0)
                    continue;

                int neighborRow = row + i;
                int neighborCol = col + j;

                if (neighborRow >= 0 && neighborRow < rowsCount
                    && neighborCol >= 0 && neighborCol < colsCount)
                {
                    if (VisibilityMap[neighborRow, neighborCol] > 0)
                        return true;
                }
            }
        }

        return false;
    }

    private void Start() {
        UpdateLightSources();
    }

    private void UpdateVisibilityMap() {
        var gridRectInScreen = GetGridRectInScreen();

        if (VisibilityMap == null) {
            VisibilityMap = new();
        }

        VisibilityMap.ScreenSizeInCells = new(
            gridRectInScreen.width,
            gridRectInScreen.height
        );
        VisibilityMap.MinCell = new(
            gridRectInScreen.xMin,
            gridRectInScreen.yMin
        );
    }

    private float CalculateVisibility(int x, int y, bool ignoreAccessibility = false) {
        Vector3Int cellPosInGrid = new(x, y);
        Vector3 cellPosInWorld = GridManager.GetCellCenterWorld(cellPosInGrid);
    
        bool shouldGetVisibility = ignoreAccessibility || IsPathClear(
            Observer.position,
            cellPosInWorld,
            cellPosInGrid);
        float visibility = shouldGetVisibility ?
            CalculateLight(cellPosInGrid, cellPosInWorld, ignoreAccessibility)
            : 0f;
        return visibility;
    }

    // private bool IsFirstObstacle(
    //     Vector3 origin,
    //     Vector3 destination,
    //     Vector3Int destinationGridPos) {
    //     Vector3 direction = destination - origin;
    //     float distance = direction.magnitude;
    //     direction.Normalize();

    //     RaycastHit2D hit = Physics2D.Raycast(
    //         origin,
    //         direction,
    //         distance,
    //         1 << LayerMask.NameToLayer("Obstacle"));
        
    //     if (GridManager.WorldToCell(hit.point) == destinationGridPos)
    //         return true;
        
    //     return false;

    //     // RaycastHit2D[] hits = Physics2D.RaycastAll(
    //     //     origin,
    //     //     direction,
    //     //     distance,
    //     //     1 << LayerMask.NameToLayer("Obstacle"));

    //     // var sortedHits = hits
    //     //     .OrderBy(hit => Vector3.Distance(
    //     //         origin,
    //     //         hit.point))
    //     //     .ToList();
    //     // var closestHit = sortedHits.FirstOrDefault();

    //     // Vector3Int hitCell = gridManager.WorldToCell(closestHit.point);

    //     // Debug.Log($"{hitCell} and {destinationGridPos}");
    //     // return hitCell == destinationGridPos;
    // }

    /// <summary>
    /// Calculates light (does not take into account visibility)
    /// </summary>
    private float CalculateLight(
        Vector3Int cellPos,
        Vector3 cellPosInWorld,
        bool ignoreAccessibility = false) {
        var sources = GetLightSources(cellPos, cellPosInWorld, ignoreAccessibility);
        if (!sources.Any())
            return 0f;
            
        return sources
            .Select(source => CalculateLightForSource(source, cellPosInWorld))
            .Max();
    }

    private float CalculateLightForSource(
        GridLightSource lightSource,
        Vector3 cellPosInWorld) {
        return LightingUtils.CalculateLighting(
            lightSource.transform.position,
            cellPosInWorld,
            lightSource.fullyVisibleRadius,
            lightSource.radius);
    }

    /// <summary>
    /// Gets the sources that light the specified cell
    /// </summary>
    private IEnumerable<GridLightSource> GetLightSources(
        Vector3Int cellPos,
        Vector3 cellPosInWorld,
        bool ignoreAccessibility) {
        // Todo: use RTree
        var query = lightSources
            .Where(source =>
                source.Bounds.Overlaps(new RectInt(cellPos.x, cellPos.y, 1, 1)));
        if (!ignoreAccessibility) {
            query = query.Where(source => IsPathClear(
                source.transform.position,
                cellPosInWorld,
                cellPos));
        }
        return query;
    }

    private bool IsPathClear(
        Vector3 origin,
        Vector3 target,
        Vector3Int targetPosInGrid) {
        return PathUtils.IsPathClear(
            origin,
            target,
            0.01f);
            // || IsFirstObstacle(origin, target, targetPosInGrid);
    }

    private RectInt GetGridRectInScreen() {
        Camera camera = Camera.main;
        Vector3 minWorldPos = camera
            .ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 maxWorldPos = camera
            .ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        Vector3Int minCellPos = GridManager.WorldToCell(minWorldPos);
        Vector3Int maxCellPos = GridManager.WorldToCell(maxWorldPos);

        return new RectInt(
            (Vector2Int)minCellPos,
            new(maxCellPos.x - minCellPos.x, maxCellPos.y - minCellPos.y)
        );
    }
}
