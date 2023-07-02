using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    public Vector3Int WorldToCell(Vector3 pos) {
        return grid.WorldToCell(pos);
    }
    public Vector3 GetCellCenterWorld(Vector3Int cellPos)
        => grid.GetCellCenterWorld(cellPos);

    /// <summary>
    /// Returns the position of the center of the nearest cell
    /// </summary>
    public Vector3 GetCellCenterWorld(Vector3 pos) {
        return GetCellCenterWorld(WorldToCell(pos));
    }

    // For use in other projects (including non-isometric ones)
    // there can be used universal formulas that are not tied to isometry
    #region Isometric specific code
    public Vector2 CartesianToVector2(Vector2 cart) {
        return CartesianToIsometric(cart);
    }

    public Vector2 GridDirectionToVector2(
        GridDirection orient) {
        Vector2 dir = GridDirectionToCartesianVector2(orient);
        return CartesianToIsometric(dir);
    }

    public Vector2 GridDirectionToRotated90Vector2(
        GridDirection orient) {
        Vector2 dir = Rotated(GridDirectionToCartesianVector2(orient), 90f);
        return CartesianToIsometric(dir);
    }

    private static Vector3 CartesianToIsometric(Vector3 cartPos) {
        Vector3 res = new Vector3();
        res.x = cartPos.x - cartPos.y;
        res.y = (cartPos.x + cartPos.y) / 2;
        return res;
    }

    private static Vector3 IsometricToCartesian(Vector3 isoPos) {
        Vector3 res = new Vector3();
        res.x = (2 * isoPos.y + isoPos.x) / 2;
        res.y = (2 * isoPos.y - isoPos.x) / 2;
        return res;
    }
    #endregion

    private const string unknownDirectionMessage = "Unknown IsometricDirection";

    private static Vector2 GridDirectionToCartesianVector2(
        GridDirection orient) {
        Vector2 dir = orient switch {
            GridDirection.North => Vector2.up,
            GridDirection.East => Vector2.right,
            GridDirection.South => Vector2.down,
            GridDirection.West => Vector2.left,

            GridDirection.NorthEast => Rotated(Vector2.up, -45f),
            GridDirection.NorthWest => Rotated(Vector2.up, 45f),
            GridDirection.SouthEast => Rotated(Vector2.down, 45f),
            GridDirection.SouthWest => Rotated(Vector2.down, -45f),
            
            _ => throw new System.ArgumentException(unknownDirectionMessage)
        };
        return dir;
    }

    private static Vector2 Rotated(Vector2 vector, float angleZ) {
        return Quaternion.Euler(0, 0, angleZ) * vector;
    }

    /// <summary>
    /// Converts GridDirection to Vector2Int, where x is
    /// horizontal and y is vertical coordinate
    /// </summary>
    public static Vector2Int GridDirectionToVector2Int(GridDirection dir) {
        var north = new Vector2Int(0, 1);
        var east = new Vector2Int(1, 0);
        var south = new Vector2Int(0, -1);
        var west = new Vector2Int(-1, 0);
        return dir switch {
            GridDirection.North => north,
            GridDirection.East => east,
            GridDirection.South => south,
            GridDirection.West => west,

            GridDirection.NorthEast => north + east,
            GridDirection.NorthWest => north + west,
            GridDirection.SouthEast => south + east,
            GridDirection.SouthWest => south + west,
            _ => throw new System.ArgumentException(unknownDirectionMessage)
        };
    }
}
