using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A component that allows to instantiate and set new AreaTiles for Area
/// </summary>
[RequireComponent(typeof(Area))]
public class AreaBuilder : MonoBehaviour
{
    // Todo: don't set in every AreaBuilder
    [SerializeField]
    private GameObject areaTilePrefab;

    // [SerializeField]
    // private List<Vector3Int> cellPositions;

    private GridManager gridManager;
    // Works in OnDrawGizmos
    protected GridManager GridManager {
        get {
            if (gridManager == null)
                gridManager = GameObject.Find("GridManager")
                    .GetComponent<GridManager>();
            return gridManager;
        }
    }

    private Area area;
    private void Awake() {
        area = GetComponent<Area>();
    }

    public void BuildAndSetAreaTiles(IEnumerable<Vector3Int> cellPositions,
        bool destroyOldAreaTiles = true) {
        var oldAreaTiles = area.AreaTiles;
        
        // Map AreaTiles to cell positions
        var areaTiles = cellPositions.Select((Vector3Int cellPos) => {
            Vector3 posInWorld = GridManager.GetCellCenterWorld(cellPos);
            GameObject areaTile = Instantiate(areaTilePrefab, posInWorld,
                Quaternion.identity);
            areaTile.transform.SetParent(transform);
            return areaTile.GetComponent<AreaTile>();
        }).ToList();
        area.SetAreaTiles(areaTiles);

        if (oldAreaTiles != null && destroyOldAreaTiles) {
            foreach (AreaTile oldAreaTile in oldAreaTiles) {
                Destroy(oldAreaTile.gameObject);
            }
        }
    }

    // private void OnDrawGizmos() {
    //     if (cellPositions == null)
    //         return;
        
    //     Color prevCol = Gizmos.color;
    //     Gizmos.color = Color.green;
    //     foreach (Vector3Int cellPos in cellPositions) {
    //         Vector3 posInWorld = GridManager.GetCellCenterWorld(cellPos);
    //         Gizmos.DrawSphere(posInWorld, 0.1f);
    //     }
    //     Gizmos.color = prevCol;
    // }
}
