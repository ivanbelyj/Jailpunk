using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class GridLightSource : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum distance in grid cells at which an object remains"
        + "fully visible without any darkness applied. Should be less than"
        + nameof(radius) + ".")]
    public int fullyVisibleRadius = 3;

    [SerializeField]
    [Tooltip("The distance in grid cells from which the visibility of an object "
        + "starts to gradually decrease until it becomes completely dark.")]
    public int radius = 6;

    public GridTransform GridTransform { get; private set; }

    /// <summary>
    /// Bounds of light source in a grid
    /// </summary>
    public RectInt Bounds {
        get {
            var pos = GridTransform.PositionInGrid;
            return new RectInt(
                pos.x - radius,
                pos.y - radius,
                radius * 2,
                radius * 2
            );
        }
    }

    private void Awake() {
        GridTransform = GetComponent<GridTransform>();
    }
}
