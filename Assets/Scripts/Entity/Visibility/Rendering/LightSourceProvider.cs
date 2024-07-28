using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTree;
using UnityEngine;

public class LightSourceProvider
{
    private RTree<GridLightSource> lightSources = new();
    private List<GridLightSource> dynamicLightSources = new();

    private HashSet<int> addedInstanceIds = new();

    private GridLightManager gridLightManager;
    private GridLightManager GridLightManager {
        get {
            if (gridLightManager == null) {
                gridLightManager = Object.FindObjectOfType<GridLightManager>();
            }
            return gridLightManager;
        }
    }

    public IEnumerable<GridLightSource> GetLightSourcesAtCell(Vector3Int cellPos) {
        var res = lightSources
            .Search(new() {
                MinX = cellPos.x,
                MaxX = cellPos.x,
                MinY = cellPos.y,
                MaxY = cellPos.y
            })
            .ToList();

        res
            .AddRange(dynamicLightSources
            .Where(source =>
                source.Bounds.Overlaps(new RectInt(cellPos.x, cellPos.y, 1, 1))));

        return res;
    }

    public void Initialize() {
        GridLightManager.LightSourceAdded += OnGridLightAdded;
        GridLightManager.LightSourceRemoved += OnGridLightRemoved;
        UpdateLightSources();
    }

    private void OnGridLightAdded(object sender, GridLightSource lightSource) {
        Debug.Log("Light source added to provider: " + lightSource.name);
        if (lightSource.isStatic) {
            lightSources.Insert(lightSource);
        } else {
            dynamicLightSources.Add(lightSource);
        }
        addedInstanceIds.Add(lightSource.GetInstanceID());
    }

    private void OnGridLightRemoved(object sender, GridLightSource lightSource) {
        if (lightSource.isStatic) {
            lightSources.Delete(lightSource);
        } else {
            dynamicLightSources.Remove(lightSource);
        }
        addedInstanceIds.Remove(lightSource.GetInstanceID());
    }

    private void UpdateLightSources() {
        var notAddedSources = Object
            .FindObjectsOfType<GridLightSource>()
            .Where(x => !addedInstanceIds.Contains(x.GetInstanceID()));

        lightSources.BulkLoad(notAddedSources.Where(x => x.isStatic));
        
        dynamicLightSources.AddRange(notAddedSources.Where(x => !x.isStatic));
    }
}
