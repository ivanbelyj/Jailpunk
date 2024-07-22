using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridGraphLoader))]
public class NavManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private float cellSize = 1f;

    public GridGraph Graph { get; private set; }

    private void Start() {
        Graph = GetComponent<GridGraphLoader>()
            .InstantiateFromTilemap(tilemap, cellSize);
    }
}
