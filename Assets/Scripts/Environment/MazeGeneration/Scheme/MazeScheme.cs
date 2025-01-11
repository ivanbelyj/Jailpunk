using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Represents the design (or plan) of the maze
/// </summary>
public class MazeScheme
{
    private readonly SchemeTile[,] map;
    public List<SchemeArea> Areas { get; set; } = new();

    private readonly DebugColorManager colorManager = new();
    private readonly SchemeStringBuilder schemeStringBuilder;

    public MazeScheme(Vector2Int mapSize)
    {
        map = InitializeMap(mapSize);
        schemeStringBuilder = new SchemeStringBuilder(this, colorManager);
    }

    public Vector2Int MapSize => new(map.GetLength(1), map.GetLength(0));

    public SchemeTile GetTileByPos(int x, int y)
    {
        return IsValidPosition(x, y) ? map[y, x] : CreateDefaultTile();
    }

    public void AddDebugMark(Vector2Int pos, Color? color = null)
    {
        colorManager.AddMark(pos, color ?? Color.red);
    }

    public void AddDebugSectorColor(int sectorId, Color color)
    {
        colorManager.AddSectorColor(sectorId, color);
    }

    public void AddDebugAreaColor(int areaId, Color color)
    {
        colorManager.AddAreaColor(areaId, color);
    }

    public void ClearDebugMarks()
    {
        colorManager.ClearAll();
    }

    public override string ToString()
    {
        return schemeStringBuilder.Render();
    }

    private SchemeTile[,] InitializeMap(Vector2Int mapSize)
    {
        var newMap = new SchemeTile[mapSize.y, mapSize.x];
        for (int row = 0; row < mapSize.y; row++)
        {
            for (int col = 0; col < mapSize.x; col++)
            {
                newMap[row, col] = CreateDefaultTile();
            }
        }
        return newMap;
    }

    private SchemeTile CreateDefaultTile()
    {
        return new SchemeTile
        {
            SectorId = null,
            TileType = TileType.NoSpace
        };
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < MapSize.x && y >= 0 && y < MapSize.y;
    }
}