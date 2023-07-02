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

    public void CreateSurroundingArea() {
        List<Vector3Int> interactionCells = GetSurroundingCellPositions();
        if (createCenterArea) {
            Vector3Int centerCell = GridManager.WorldToCell(transform.position);
            interactionCells.Add(centerCell);
        }
        areaBuilder.BuildAndSetAreaTiles(interactionCells);
    }

    private void OnDrawGizmos() {
        var interactionCellsPos = GetSurroundingCellPositions();
        if (interactionCellsPos == null)
            return;

        Color prevColor = Gizmos.color;
        Gizmos.color = Color.green;
        foreach (Vector3Int cellPos in interactionCellsPos) {
            Gizmos.DrawLine(GridManager.GetCellCenterWorld(transform.position),
                GridManager.GetCellCenterWorld(cellPos));
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
            
            Vector3Int surroundingCell = GridManager.WorldToCell(transform.position)
                + (Vector3Int)GridUtils.GridDirectionToVector2Int(dir);
            // Vector3 cellWorldPos = gridManager.GetCellCenterWorld(
            //     (Vector3Int)neighborCell);
            res.Add(surroundingCell);
            addedDirections.Add(dir);
        }
        return res;
    }
}
