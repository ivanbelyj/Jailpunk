using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppearanceSpriteResolvingUtils 
{
    public static string GetCategoryName(string name, string state, int? angle) =>
        $"{name}" +
        $"{(state != null ? $"_{state}" : "")}" +
        $"{(angle != null ? $"_{angle.Value:D3}" : "")}";

    public static string ToAppearanceDataId(AppearanceSpriteData data) =>
        $"{data.Name}" +
        $"{(data.State != null ? $"_{data.State}" : "")}" +
        $"{(data.Angle != null ? $"_{data.Angle.Value:D3}" : "")}" +
        $"{(data.Index != null ? $"_{data.Index.Value:D3}" : "")}";
}
