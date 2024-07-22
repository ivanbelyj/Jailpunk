using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGraphLoader : MonoBehaviour
{
    [SerializeField]
    [Range(0, Mathf.Infinity)]
    private float defaultCost = 1f;

    [SerializeField]
    [Range(0, Mathf.Infinity)]
    private float maximumCost = Mathf.Infinity;

    [SerializeField]
    private GameObject vertexPrefab;

    [SerializeField]
    private GameObject obstaclePrefab;

    private GridManager gridManager;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
    }

    public GridGraph InstantiateFromTilemap(Tilemap tilemap, float cellSize)
    {
        BoundsInt bounds = tilemap.cellBounds;

        int numRows = bounds.size.y;
        int numCols = bounds.size.x;

        List<Vertex> vertices = new List<Vertex>(numRows * numCols);
        List<List<Vertex>> neighbours = new List<List<Vertex>>(numRows * numCols);
        List<List<float>> costs = new List<List<float>>(numRows * numCols);
        GameObject[] vertexObjs = new GameObject[numRows * numCols];
        bool[,] mapVertices = new bool[numRows, numCols];

        Vector2 mapOrigin = GetTilemapOrigin(tilemap);
        GridGraph graph = new(
            cellSize,
            mapOrigin,
            mapVertices,
            vertices,
            neighbours,
            costs,
            defaultCost,
            maximumCost);

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                Vector3Int localPlace = new Vector3Int(x, y, bounds.min.z);
                Vector3 worldPos = gridManager.GetCellCenterWorld(localPlace);
                worldPos.z = 0;

                int id = graph.GridToId(x - bounds.min.x, y - bounds.min.y);

                GameObject prefab = obstaclePrefab;
                TileBase tile = tilemap.GetTile(localPlace);
                if (tile == null)
                {
                    prefab = vertexPrefab;
                    mapVertices[y - bounds.min.y, x - bounds.min.x] = true;
                }

                vertexObjs[id] = Instantiate(prefab, worldPos, Quaternion.identity);
                vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                Vertex v = vertexObjs[id].AddComponent<Vertex>();
                v.Id = id;
                vertices.Add(v);
                neighbours.Add(new List<Vertex>());
                costs.Add(new List<float>());

                float yScale = vertexObjs[id].transform.localScale.y;
                Vector3 scale = new Vector3(cellSize, yScale, cellSize);
                vertexObjs[id].transform.localScale = scale;
                vertexObjs[id].transform.parent = tilemap.gameObject.transform;
            }
        }

        graph.SetNeighbours();

        return graph;
    }

    private Vector3 GetTilemapOrigin(Tilemap tilemap) {
        return gridManager.CellToWorld(tilemap.cellBounds.min);
    }
}
