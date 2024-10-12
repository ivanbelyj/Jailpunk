using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLibraryUtils 
{
    public static string GetCategoryName(
        string animationBase,
        string state,
        int? angle) {
        string res = $"{animationBase}";
        if (state != null) {
            res += $"-{state}";
        }
        if (angle != null) {
            res += $"-{angle.Value:D3}";
        }
        return res;
    }
}
