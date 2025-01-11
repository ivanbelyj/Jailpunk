using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages debug colors for marks, sectors, and areas
/// </summary>
public class DebugColorManager
{
    private readonly Dictionary<Vector2Int, Color> markedPositions = new();
    private readonly Dictionary<int, Color> sectorColors = new();
    private readonly Dictionary<int, Color> areaColors = new();

    public void AddMark(Vector2Int position, Color color)
    {
        if (!markedPositions.TryAdd(position, color))
        {
            Debug.LogWarning($"Override debug color mark (position: {position})");
            markedPositions[position] = color;
        }
    }

    public void AddSectorColor(int sectorId, Color color)
    {
        if (!sectorColors.TryAdd(sectorId, color))
        {
            Debug.LogWarning($"Override sector debug color (sector id: {sectorId})");
            sectorColors[sectorId] = color;
        }
    }

    public void AddAreaColor(int areaId, Color color)
    {
        if (!areaColors.TryAdd(areaId, color))
        {
            Debug.LogWarning($"Override area debug color (area id: {areaId})");
            areaColors[areaId] = color;
        }
    }

    public string GetTileColorCode(SchemeTile tile, Vector2Int position)
    {
        if (markedPositions.TryGetValue(position, out var markColor))
        {
            return ColorUtility.ToHtmlStringRGB(markColor);
        }

        if (tile.AreaId.HasValue && areaColors.TryGetValue(tile.AreaId.Value, out var areaColor))
        {
            return ColorUtility.ToHtmlStringRGB(areaColor);
        }

        if (tile.SectorId.HasValue && sectorColors.TryGetValue(tile.SectorId.Value, out var sectorColor))
        {
            return ColorUtility.ToHtmlStringRGB(sectorColor);
        }

        return null;
    }

    public void ClearAll()
    {
        markedPositions.Clear();
        sectorColors.Clear();
        areaColors.Clear();
    }
}
