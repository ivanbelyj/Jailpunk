using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Represents the design (or plan) of the maze
/// </summary>
public class MazeScheme
{
    private SchemeTile[,] Map { get; set; }

    /// <summary>
    /// Maze scheme positions marked for debug
    /// </summary>
    private Dictionary<Vector2Int, Color> ColorsByMarkedPosition = new();

    private Dictionary<int, Color> ColorsBySectorId = new();

    public MazeScheme(Vector2Int mapSize) {
        Map = new SchemeTile[mapSize.y, mapSize.x];
        for (int row = 0; row < mapSize.y; row++) {
            for (int col = 0; col < mapSize.x; col++) {
                Map[row, col] = new SchemeTile() {
                    SectorId = null,
                    TileType = TileType.NoSpace
                };
            }
        }
    }

    public SchemeTile GetTileByPos(int x, int y)
    {
        // Todo: temporary solution ?
        if (x < 0 || x >= Map.GetLength(1) || y < 0 || y >= Map.GetLength(0))
        {
            return new SchemeTile() {
                SectorId = null,
                TileType = TileType.NoSpace
            };
        }

        return Map[y, x];
    }

    public Vector2Int MapSize => new Vector2Int(Map.GetLength(1), Map.GetLength(0));

    public void AddDebugMark(Vector2Int pos, Color? color = null) {
        if (color == null) {
            color = Color.red;
        }
        if (!ColorsByMarkedPosition.ContainsKey(pos)) {
            ColorsByMarkedPosition.Add(pos, color.Value);
        } else {
            Debug.LogWarning($"Override debug color mark (position: {pos})");
            ColorsByMarkedPosition[pos] = color.Value;
        }
    }

    public void AddDebugSectorColor(int sectorId, Color color) {
        if (!ColorsBySectorId.ContainsKey(sectorId)) {
            ColorsBySectorId.Add(sectorId, color);
        } else {
            Debug.LogWarning($"Override sector debug color (sector: {sectorId})");
            ColorsBySectorId[sectorId] = color;
        }
    }

    public void ClearDebugMarks() {
        ColorsByMarkedPosition = new();
        ColorsBySectorId = new();
    }

    // Todo: Refactor

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();

        for (int y = 0; y < MapSize.y; y++)
        {
            for (int x = 0; x < MapSize.x; x++)  
            {
                SchemeTile tile = GetTileByPos(x, y);
                string colorCode = null;
                var posVector = new Vector2Int(x, y);
                if (ColorsByMarkedPosition.ContainsKey(posVector)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(ColorsByMarkedPosition[posVector]);
                }
                if (tile.SectorId != null
                    && ColorsBySectorId.ContainsKey(tile.SectorId.Value)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(ColorsBySectorId[tile.SectorId.Value]);
                }
                if (colorCode != null) {
                    strBuilder.Append($"<b><color=#{colorCode}>");
                }
                    
                // █ █ █
                strBuilder.Append(TileToString(tile, colorCode));
                if (colorCode != null) {
                    strBuilder.Append("</color></b>");
                }
            }
            strBuilder.Append("\n");
        }
        return strBuilder.ToString();
    }

    // Todo: more convenient way of control
    private const bool DisplaySectorIds = false;

    private string TileToString(SchemeTile tile, string colorCode) {
        return tile.TileType switch {
            TileType.NoSpace => colorCode != null ? ".." : "  ",
            TileType.Floor => DisplaySectorIds ? ToSectorString(tile) : "..",
            TileType.LoadBearingWall => DisplaySectorIds ? ToSectorString(tile) : "==",
            TileType.Wall => DisplaySectorIds ? ToSectorString(tile) : "--",
            _ => "??"
        };
    }
    
    private string ToSectorString(SchemeTile tile) 
       => tile.SectorId == null ? "??" : $"{tile.SectorId.Value:D2}".Substring(0, 2);
}
