using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Represents the design of the maze
/// </summary>
public class MazeScheme
{
    private SchemeTile[,] Map { get; set; }

    /// <summary>
    /// Maze scheme positions marked for debug
    /// </summary>
    private Dictionary<Vector2Int, Color> ColorsByMarkedPositions =
        new Dictionary<Vector2Int, Color>();

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

    public SchemeTile GetTileByPos(int x, int y) => Map[y, x];

    public Vector2Int MapSize => new Vector2Int(Map.GetLength(1), Map.GetLength(0));

    public void AddDebugMark(Vector2Int pos, Color? color = null) {
        if (color == null)
            color = Color.red;
        if (!ColorsByMarkedPositions.ContainsKey(pos)) {
            ColorsByMarkedPositions.Add(pos, color.Value);
        }
    }
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
                if (ColorsByMarkedPositions.ContainsKey(posVector)) {
                    colorCode = ColorUtility
                        .ToHtmlStringRGB(ColorsByMarkedPositions[posVector]);
                }
                if (colorCode != null)
                    strBuilder.Append($"<b><color=#{colorCode}>");
                strBuilder.Append(tile.TileType switch {
                    TileType.NoSpace => "  ",
                    TileType.Floor => "..",
                    TileType.LoadBearingWall => "==",
                    TileType.Wall => "--",
                    _ => "??"
                });
                if (colorCode != null)
                    strBuilder.Append("</color></b>");
            }
            strBuilder.Append("\n");
        }
        return strBuilder.ToString();
    }
}
