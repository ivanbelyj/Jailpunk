using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component of GameObject that has an orientation in the grid
/// </summary>
public class GridOriented : MonoBehaviour
{
    [SerializeField]
    protected GridDirection orientation;
    public GridDirection Orientation {
        get => orientation;
        set => orientation = value;
    }

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

    public virtual void OnDrawGizmos() {
        Color prevColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(GridManager.GetCellCenterWorld(transform.position), Forward);
        Gizmos.color = prevColor;
    }

    public Vector3 Forward => GridManager
        .GridDirectionToGridVector(orientation).normalized;

    public Vector3 Rotated90 => gridManager
        .GridDirectionToRotated90GridVector(orientation).normalized;
}
