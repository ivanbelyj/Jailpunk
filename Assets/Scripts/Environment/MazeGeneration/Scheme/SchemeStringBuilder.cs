using System.Text;
using UnityEngine;

/// <summary>
/// Responsible for rendering the maze into a string representation
/// </summary>
public class SchemeStringBuilder
{
    private const bool DisplayAreaIds = false;
    private readonly MazeScheme mazeScheme;
    private readonly DebugColorManager colorManager;

    public SchemeStringBuilder(MazeScheme mazeScheme, DebugColorManager colorManager)
    {
        this.mazeScheme = mazeScheme;
        this.colorManager = colorManager;
    }

    public string Render()
    {
        StringBuilder strBuilder = new();

        for (int y = 0; y < mazeScheme.MapSize.y; y++)
        {
            for (int x = 0; x < mazeScheme.MapSize.x; x++)
            {
                AppendTileRepresentation(strBuilder, x, y);
            }
            strBuilder.AppendLine();
        }

        return strBuilder.ToString();
    }

    private void AppendTileRepresentation(StringBuilder builder, int x, int y)
    {
        var tile = mazeScheme.GetTileByPos(x, y);
        string colorCode = colorManager.GetTileColorCode(tile, new Vector2Int(x, y));

        if (!string.IsNullOrEmpty(colorCode)) {
            builder.Append($"<color=#{colorCode}>");
        }

        builder.Append(TileToString(tile));

        if (!string.IsNullOrEmpty(colorCode)) {
            builder.Append("</color>");
        }
    }

    private string TileToString(SchemeTile tile)
    {
        return tile.TileType switch
        {
            TileType.NoSpace => "  ",
            TileType.Floor => DisplayAreaIds ? ToAreaString(tile) : "..",
            TileType.LoadBearingWall => DisplayAreaIds ? ToAreaString(tile) : "##",
            TileType.Wall => DisplayAreaIds ? ToAreaString(tile) : "--",
            _ => "??"
        };
    }

    private string ToAreaString(SchemeTile tile)
    {
        return tile.AreaId == null ? "??" : $"{tile.AreaId.Value:D2}".Substring(0, 2);
    }
}
