using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppearanceSpriteResolvingUtils 
{
    public static string GetCategoryName(string name, string state, int? angle) =>
        $"{name}" +
        $"{(state != null ? $"_{state}" : "")}" +
        $"{(angle != null ? $"_{angle.Value:D3}" : "")}";
}
