using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleHelper
{
    private Grid grid;

    private List<Tilemap> obstacleTilemaps = null;
    private bool isCacheValid = false;

    public ObstacleHelper(Grid grid)
    {
        this.grid = grid;
    }

    public List<Tilemap> GetObstacleTilemaps()
    {
        if (!isCacheValid)
        {
            UpdateObstacleTilemapsCache();
        }

        return obstacleTilemaps;
    }

    private void UpdateObstacleTilemapsCache()
    {
        obstacleTilemaps = new List<Tilemap>();
        foreach (Transform child in grid.transform)
        {
            Tilemap tilemap = child.GetComponent<Tilemap>();
            if (tilemap != null && child.gameObject.layer == LayerMask.NameToLayer("VisionObstacle"))
            {
                obstacleTilemaps.Add(tilemap);
            }
        }
        isCacheValid = true;
    }

    public void InvalidateCache()
    {
        isCacheValid = false;
    }

    public bool IsObstacle(Vector3 worldPosition)
    {
        Vector3Int cellPosition = grid.WorldToCell(worldPosition);
        foreach (Tilemap tilemap in GetObstacleTilemaps())
        {
            TileBase tile = tilemap.GetTile(cellPosition);
            if (tile != null)
            {
                return true; 
            }
        }

        return false; 
    }
}
