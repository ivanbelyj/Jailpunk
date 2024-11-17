using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Component for objects positioned and oriented in a grid
/// </summary>
public class GridTransform : NetworkBehaviour
{
    [SerializeField]
    private GridDirection initialOrientation;

    [SyncVar]
    private GridDirection orientation;
    public GridDirection Orientation {
        get => orientation;
    }

    [SerializeField]
    private double setOrientationDelay = 0.1f;

    /// <summary>
    /// Position in grid
    /// </summary>
    public Vector3Int PositionInGrid {
        get => GridManager.WorldToCell(transform.position);
    }

    private GridManager gridManager;
    private GridManager GridManager {
        get {
            const string GridManagerName = "GridManager";
            if (gridManager == null) {
                gridManager = GameObject
                    .Find(GridManagerName)
                    ?.GetComponent<GridManager>();
                if (gridManager == null) {
                    Debug.LogError(
                    $"{nameof(GridManager)} was not found (name: {GridManagerName})");
                }
            }
            return gridManager;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SetOrientation(initialOrientation, false);
    }

    public void SetOrientation(GridDirection orientation, bool useDelay) {
        if (isServer) {
            SetOrientationCore(orientation, useDelay);
        } else {
            CmdSetOrientation(orientation, useDelay);
        }
    }

    [Command]
    private void CmdSetOrientation(GridDirection orientation, bool useDelay) {
        SetOrientationCore(orientation, useDelay);
    }

    private double orientationSetTime;

    private void SetOrientationCore(GridDirection orientation, bool useDelay) {
        if (orientation == this.orientation) {
            orientationSetTime = NetworkTime.time;
        }
        if (!useDelay || NetworkTime.time - orientationSetTime > setOrientationDelay) {
            this.orientation = orientation;
        }
    }

    public virtual void OnDrawGizmos() {
        if (GridManager == null) {
            return;
        }

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
