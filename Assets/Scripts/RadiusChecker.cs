using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using System;

/// <summary>
/// Checks for objects in the specified radius in Update and provides according events
/// </summary>
public class RadiusChecker : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;
    public float Radius {
        get => radius;
        set => radius = value;
    }

    /// <summary>
    /// The last objects in radius
    /// </summary>
    private HashSet<GameObject> lastInRadius = new HashSet<GameObject>();
    public HashSet<GameObject> LastInRadius => lastInRadius;

    // Works in OnDrawGizmos
    private GridManager gridManager;
    private GridManager GridManager {
        get {
            if (gridManager == null)
                gridManager = GameObject.Find("GridManager")
                    .GetComponent<GridManager>();
            return gridManager;
        }
    }

    /// <summary>
    /// Fires when there are new objects in the radius
    /// </summary>
    public event Action<List<GameObject>> NewInRadius;

    /// <summary>
    /// Fires when some objects have gone out of the radius
    /// </summary>
    public event Action<List<GameObject>> OutOfRadius;

    private void Update() {
        CheckObjectsInRadius(false);
    }

    public bool IsInRadius(GameObject go) {
        return lastInRadius.Contains(go);
    }

    private void CheckObjectsInRadius(bool drawGizmos = false) {
        Vector2 checkerPos = transform.position;
        // Todo: Physics2D.OverlapCircleNonAlloc ?
        Collider2D[] overlappedByCircle = Physics2D.OverlapCircleAll(
            checkerPos,
            radius);
        var objectsInRadius = overlappedByCircle
            .Where(collider => {
                // Check is object in radius
                Vector2 objectPos = collider.transform.position;
                bool isDistanceLess = IsDistanceInGridLessThanRadius(
                    checkerPos, objectPos);
                
                if (drawGizmos) {
                    Color prevCol = Gizmos.color;
                    Gizmos.color = isDistanceLess ? Color.green : Color.red;
                    Gizmos.DrawLine(objectPos, checkerPos);
                    Gizmos.color = prevCol;
                }
                
                return isDistanceLess;
            });
        
        // Collect new objects in the radius
        var newInRadius = new List<GameObject>();
        var objectsInRadiusSet = new HashSet<GameObject>();
        foreach (Collider2D collider in objectsInRadius) {
            var go = collider.gameObject;
            objectsInRadiusSet.Add(go);
            if (!lastInRadius.Contains(go)) {
                newInRadius.Add(go);
            }
        }
        foreach (var go in newInRadius) {
            lastInRadius.Add(go);
        }

        // Collect objects that were in the radius, but now have gone
        var outOfRadius = new List<GameObject>();
        foreach (GameObject go in lastInRadius) {
            if (!objectsInRadiusSet.Contains(go)) {
                outOfRadius.Add(go);
            }
        }
        foreach (var go in outOfRadius) {
            lastInRadius.Remove(go);
        }

        if (outOfRadius.Count > 0)
            OutOfRadius?.Invoke(outOfRadius);
        if (newInRadius.Count > 0)
            NewInRadius?.Invoke(newInRadius);
    }

    private bool IsDistanceInGridLessThanRadius(Vector2 v1, Vector2 v2) {
        Vector2 v1InGrid = GridManager.Vector2ToCartesian(v1);
        Vector2 v2InGrid = GridManager.Vector2ToCartesian(v2);
        return Vector2.Distance(v1InGrid, v2InGrid) < radius;
    }

    private void OnDrawGizmos() {
        CheckObjectsInRadius(true);
    }
}
