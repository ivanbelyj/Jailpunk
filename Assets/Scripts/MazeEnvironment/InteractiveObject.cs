using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField]
    private GameObject areaPrefab;

    [SerializeField]
    [Tooltip("Interactable neighbor tiles. Multiple identical directions"
        + " have no side effects")]
    private GridDirection[] interactableDirections;

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

    private void Awake() {
        CreateInteractionCells();
    }

    private void CreateInteractionCells() {
        var interactionCells = GetInteractionCellsPosInWorld();
        // Todo: instantiate
    }

    private void OnDrawGizmos() {
        var interactionCellsPos = GetInteractionCellsPosInWorld();
        if (interactionCellsPos == null)
            return;

        Color prevColor = Gizmos.color;
        Gizmos.color = Color.green;
        foreach (Vector3 cellPos in interactionCellsPos) {
            Gizmos.DrawLine(gridManager.ToCellCenter(transform.position), cellPos);
        }
        Gizmos.color = prevColor;
    }

    private List<Vector3> GetInteractionCellsPosInWorld() {
        if (interactableDirections == null)
            return null;
        
        var res = new List<Vector3>();
        var addedDirections = new HashSet<GridDirection>();
        foreach (GridDirection dir in interactableDirections) {
            if (addedDirections.Contains(dir))
                continue;
            
            Vector2Int neighborCell = (Vector2Int)GridManager
                .WorldToCell(transform.position)
                + GridUtils.GridDirectionToVector2Int(dir);
            Vector3 cellWorldPos = gridManager.GetCellCenterWorld(
                (Vector3Int)neighborCell);
            res.Add(cellWorldPos);
            addedDirections.Add(dir);
        }
        return res;
    }
}
