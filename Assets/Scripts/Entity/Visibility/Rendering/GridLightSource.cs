using System.Collections;
using System.Collections.Generic;
using RTree;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class GridLightSource : MonoBehaviour, ISpatialData
{
    [Tooltip("Check this option for light sources that do not change position "
        + "during gameplay.\n Marking them as static allows to apply optimization "
        + "that can improve performance,\n"
        + "especially in scenes with many light sources.")]
    public bool isStatic = false;

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

    private GridLightManager gridLightManager;
    private GridLightManager GridLightManager {
        get {
            if (gridLightManager == null) {
                gridLightManager = FindObjectOfType<GridLightManager>();
            }
            return gridLightManager;
        }
    }

    // private bool isFirstEnable = true;

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

    public Envelope Envelope {
        get {
            var bounds = Bounds;
            return new(bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax);
        }
    }

    private void Start() {
        GridTransform = GetComponent<GridTransform>();
        if (GridLightManager == null) {
            Debug.LogError($"{nameof(GridLightManager)} was not found on scene. "
                + $"Please, add it to use {nameof(GridLightSource)} properly");
        }
        GridLightManager.AddLightSource(this);
    }

    // Todo: enable / disable / destroy support

    // private void OnEnable() {
    //     if (isFirstEnable) {
    //         // Ignore on first enable (light source is added on Start)
    //         isFirstEnable = false;
    //     } else {
    //         GridLightManager.AddLightSource(this);
    //     }
    // }

    // private void OnDisable()
    // {
    //     GridLightManager.RemoveLightSource(this);
    // }

    // private void OnDestroy()
    // {
    //     GridLightManager.RemoveLightSource(this);
    // }
}
