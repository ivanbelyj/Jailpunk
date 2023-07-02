using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Some tiles united in one logical area for detecting
/// collision enters and exits
/// </summary>
public class Area : MonoBehaviour
{
    [SerializeField]
    private List<AreaTile> initialAreaTiles;

    private IEnumerable<AreaTile> areaTiles;
    public IEnumerable<AreaTile> AreaTiles {
        get => areaTiles;
    }

    private HashSet<GameObject> gameObjectsInArea;

    public event Action<GameObject> AreaEntered;
    public event Action<GameObject> AreaExited;

    private void Awake() {
        if (initialAreaTiles != null && initialAreaTiles.Count > 0)
            SetAreaTiles(initialAreaTiles);
    }

    /// <summary>
    /// Resets Area with new AreaTiles
    /// </summary>
    public void SetAreaTiles(IEnumerable<AreaTile> areaTiles) {
        // Detach old AreaTiles
        var oldAreaTiles = this.areaTiles;
        if (oldAreaTiles != null) {
            foreach (AreaTile areaTile in oldAreaTiles) {
                areaTile.AreaTileEntered -= OnAreaTileEntered;
                areaTile.AreaTileExited -= OnAreaTileExited;
            }
        }

        // Attach new AreaTile
        this.areaTiles = areaTiles;

        gameObjectsInArea = new HashSet<GameObject>();
        foreach (AreaTile areaTile in areaTiles) {
            areaTile.AreaTileEntered += OnAreaTileEntered;
            areaTile.AreaTileExited += OnAreaTileExited;
        }
        AreaEntered += (go) => {
            Debug.Log(go.name + " enters the area");
        };
        AreaExited += (go) => {
            Debug.Log(go.name + " leaves the area");
        };
    }

    private void OnAreaTileEntered(GameObject go) {
        if (gameObjectsInArea.Contains(go))
            return;  // This object is already in area, nothing happened
        
        gameObjectsInArea.Add(go);
        AreaEntered?.Invoke(go);
    }

    private void OnAreaTileExited(GameObject go) {
        // Check is it anywhere else
        foreach (AreaTile areaTile in areaTiles) {
            if (areaTile.IsInArea(go))
                return;  // Nothing happened
        }

        // There is no GameObject in the Area
        gameObjectsInArea.Remove(go);
        AreaExited?.Invoke(go);
    }
}
