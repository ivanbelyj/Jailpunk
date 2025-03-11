using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridGraphLoader))]
public class NavManager : NavManagerCore
{
    public void Initialize(Tilemap tilemap) {
        Graph = GetComponent<GridGraphLoader>().InstantiateFromTilemap(
            tilemap,
            tilemap.cellSize.x);
    }
}
