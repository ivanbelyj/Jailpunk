using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLibraryUtils 
{
    public static string GetCategoryName(
        AnimationParameters parameters,
        int? angle = null)
        => $"{parameters.AnimationBase}-{angle ?? parameters.Angle:D3}";
}
