using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A component that instantiates surrounding area
/// </summary>
[RequireComponent(typeof(AreaBuilder))]
public class SurroundingAreaBuilder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Directions where area tiles will be instantiated. "
        + "Multiple identical directions have no side effects")]
    private GridDirection[] areaDirections;
    public GridDirection[] AreaDirections {
        get => areaDirections;
    }

    [SerializeField]
    private bool createCenterArea = true;

    [SerializeField]
    private bool buildOnStart = true;

    private GridManager gridManager;

    // Works in OnDrawGizmos
    private GridManager GridManager {
        get {
            if (gridManager == null)
                gridManager = GameObject.Find("GridManager")
                    .GetComponent<GridManager>();
            return gridManager;
        }
    }

    private AreaBuilder areaBuilder;

    private void Awake() {
        areaBuilder = GetComponent<AreaBuilder>();
    }

    private void Start() {
        if (buildOnStart)
            CreateSurroundingArea();
    }

    public void SetAreaDirectionsAndCreate(GridDirection[] newDirs) {
        areaDirections = newDirs;
        CreateSurroundingArea();
    }

    public void CreateSurroundingArea() {
        List<Vector3Int> surroundingCells = GetSurroundingCellPositions();
        if (createCenterArea) {
            Vector3Int centerCell = GridManager.WorldToCell(transform.position);
            surroundingCells.Add(centerCell);
        }
        areaBuilder.BuildAndSetAreaTiles(surroundingCells);
    }

    private void OnDrawGizmos() {
        var surroundingCellsPos = GetSurroundingCellPositions();
        if (surroundingCellsPos == null)
            return;

        Color prevColor = Gizmos.color;
        Gizmos.color = Color.green;
        Vector3 centerCellPos = GridManager.GetCellCenterWorld(transform.position);
        foreach (Vector3Int cellPos in surroundingCellsPos) {
            Gizmos.DrawLine(centerCellPos,
                GridManager.GetCellCenterWorld(cellPos));
            // Gizmos.DrawLine(GridManager.CellToWorld(centerCellPos),
            //     GridManager.CellToWorld(cellPos));
        }
        Gizmos.color = prevColor;
    }

    private List<Vector3Int> GetSurroundingCellPositions() {
        if (areaDirections == null)
            return null;
        
        var res = new List<Vector3Int>();
        var addedDirections = new HashSet<GridDirection>();
        foreach (GridDirection dir in areaDirections) {
            if (addedDirections.Contains(dir))
                continue;
            
            Vector3Int surroundingCell =
                GridManager.WorldToCell(transform.position)
                + (Vector3Int)GridDirectionUtils
                    .GridDirectionToVectorInt(dir);
            // Vector3 cellWorldPos = gridManager.GetCellCenterWorld(
            //     (Vector3Int)neighborCell);
            res.Add(surroundingCell);
            addedDirections.Add(dir);
        }
        return res;
    }
}
