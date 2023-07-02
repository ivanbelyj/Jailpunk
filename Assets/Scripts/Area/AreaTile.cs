using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaTile is an element that constitute Area
/// </summary>
public class AreaTile : MonoBehaviour
{
    private HashSet<GameObject> gameObjectsInAreaTile;

    public event Action<GameObject> AreaTileEntered;
    public event Action<GameObject> AreaTileExited;

    private void Awake() {
        gameObjectsInAreaTile = new HashSet<GameObject>();
    }

    public bool IsInArea(GameObject go) {
        return gameObjectsInAreaTile.Contains(go);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        gameObjectsInAreaTile.Add(go);
        AreaTileEntered?.Invoke(go);
    }

    private void OnTriggerExit2D(Collider2D other) {
        GameObject go = other.gameObject;
        gameObjectsInAreaTile.Remove(go);
        AreaTileExited?.Invoke(go);
    }
}
