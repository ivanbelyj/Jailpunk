using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridGraphLoader))]
public class NavManager : MonoBehaviour
{
    [SerializeField]
    private float cellSize = 1f;
    protected GridGraph graph;

    public GridGraph Graph {
        get {
            if (graph == null) {
                Debug.LogError($"{nameof(Graph)} was not initialized in {nameof(NavManager)} ");
            }
            return graph;
        }
        protected set {
            graph = value;
        }
    }

    public void Initialize(Tilemap tilemap) {
        Graph = GetComponent<GridGraphLoader>().InstantiateFromTilemap(
            tilemap,
            tilemap.cellSize.x);
    }
}
