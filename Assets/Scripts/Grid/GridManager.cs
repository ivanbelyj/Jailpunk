using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private ObstacleHelper obstacleHelper;
    private bool isInitialized = false;
    
    private Grid grid;
    private Grid Grid
    {
        get
        {
            grid ??= FindAnyObjectByType<Grid>();
            
            if (grid == null)
            {
                Debug.LogError(
                    $"{nameof(Grid)} is not found. It is required for " +
                    $"{nameof(GridManager)}");
            }
            return grid;
        }
    }

    /// <summary>
    /// A property used, in particular, in OnDrawGizmos, to check is GridManager
    /// ready to provide calculations based on the grid
    /// </summary>
    public bool IsGridSet => grid != null;

    public Vector3Int WorldToCell(Vector3 pos) {
        return Grid.WorldToCell(pos);  //  + new Vector3(-0.25f, -0.25f)
    }
    public Vector3 GetCellCenterWorld(Vector3Int cellPos)
        => Grid.GetCellCenterWorld(cellPos);
        // Isometric: 
        // //  It's not working correctly with Isometric Z as Y
        // // The hardcode float corrects the strange offset of this type of grid
        // => grid.GetCellCenterWorld(cellPos) + new Vector3(0, -0.12f);
        // // => grid.CellToWorld(cellPos) + new Vector3(0, 0.25f);

    public Vector3 CellToWorld(Vector3Int cellPos)
        => Grid.CellToWorld(cellPos);

    /// <summary>
    /// Returns the position of the center of the nearest cell
    /// </summary>
    public Vector3 GetCellCenterWorld(Vector3 pos) {
        return GetCellCenterWorld(WorldToCell(pos));
    }

    // For use in other projects (isometric or not) in the future
    // there can be used universal formulas that are not tied to isometry
    // or cartesian. Now convertions are hardcoded
    #region Grid vector convertions
    public Vector2 CartesianToGridVector(Vector2 cart) {
        // return IsometricUtils.CartesianToIsometric(cart);
        return cart;
    }

    public Vector2 GridVectorToCartesian(Vector2 cart) {
        // return IsometricUtils.IsometricToCartesian(cart);
        return cart;
    }

    public Vector2 GridDirectionToGridVector(
        GridDirection orient) {
        Vector2 dir = GridDirectionUtils.GridDirectionToCartesianVector(orient);
        // return IsometricUtils.CartesianToIsometric(dir);
        return dir;
    }
    #endregion

    public Vector2 GridDirectionToRotated90GridVector(
        GridDirection orient) {
        Vector2 dir = GridDirectionUtils.Rotated(GridDirectionUtils.GridDirectionToCartesianVector(orient), 90f);
        return CartesianToGridVector(dir);
    }

    public bool IsObstacle(Vector3Int cellPosition)
    {
        EnsureInitialized();
        return obstacleHelper.IsObstacle(cellPosition);
    }

    private void EnsureInitialized()
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }
    }

    private void Initialize()
    {
        obstacleHelper = new(Grid);
    }
}
