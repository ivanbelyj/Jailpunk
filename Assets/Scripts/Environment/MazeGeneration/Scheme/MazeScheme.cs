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
    public List<SchemeArea> Areas { get; set; } = new();

    /// <summary>
    /// Maze scheme positions marked for debug
    /// </summary>
    private Dictionary<Vector2Int, Color> colorsByMarkedPosition = new();

    private Dictionary<int, Color> colorsBySectorId = new();

    private Dictionary<int, Color> colorsByAreaId = new();

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
        if (!colorsByMarkedPosition.ContainsKey(pos)) {
            colorsByMarkedPosition.Add(pos, color.Value);
        } else {
            Debug.LogWarning($"Override debug color mark (position: {pos})");
            colorsByMarkedPosition[pos] = color.Value;
        }
    }

    public void AddDebugSectorColor(int sectorId, Color color) {
        if (!colorsBySectorId.ContainsKey(sectorId)) {
            colorsBySectorId.Add(sectorId, color);
        } else {
            Debug.LogWarning($"Override sector debug color (sector id: {sectorId})");
            colorsBySectorId[sectorId] = color;
        }
    }

    public void AddDebugAreaColor(int areaId, Color color) {
        if (!colorsByAreaId.ContainsKey(areaId)) {
            colorsByAreaId.Add(areaId, color);
        } else {
            Debug.LogWarning($"Override area debug color (area id: {areaId})");
            colorsByAreaId[areaId] = color;
        }
    }

    public void ClearDebugMarks() {
        colorsByMarkedPosition = new();
        colorsBySectorId = new();
        colorsByMarkedPosition = new();
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
                
                if (tile.SectorId != null
                    && colorsBySectorId.ContainsKey(tile.SectorId.Value)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(colorsBySectorId[tile.SectorId.Value]);
                }
                if (tile.AreaId != null
                    && colorsByAreaId.ContainsKey(tile.AreaId.Value)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(colorsByAreaId[tile.AreaId.Value]);
                }
                var posVector = new Vector2Int(x, y);
                if (colorsByMarkedPosition.ContainsKey(posVector)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(colorsByMarkedPosition[posVector]);
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
    private const bool DisplayAreaIds = false;

    private string TileToString(SchemeTile tile, string colorCode) {
        return tile.TileType switch {
            TileType.NoSpace => colorCode != null ? ".." : "  ",
            TileType.Floor => DisplayAreaIds ? ToAreaString(tile) : "..",
            TileType.LoadBearingWall => DisplayAreaIds ? ToAreaString(tile) : "##",
            TileType.Wall => DisplayAreaIds ? ToAreaString(tile) : "--",
            _ => "??"
        };
    }
    
    private string ToAreaString(SchemeTile tile) 
       => tile.AreaId == null ? "??" : $"{tile.AreaId.Value:D2}".Substring(0, 2);
}
