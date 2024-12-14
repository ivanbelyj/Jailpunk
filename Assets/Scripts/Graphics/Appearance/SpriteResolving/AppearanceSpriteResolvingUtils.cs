using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppearanceSpriteResolvingUtils 
{
    public static string GetCategoryName(string objectName, string state, int? angle) =>
        $"{objectName}" +
        $"{(state != null ? $"_{state}" : "")}" +
        $"{(angle != null ? $"_{angle.Value:D3}" : "")}";
}
