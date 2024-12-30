using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data defining the sprite to resolve
/// </summary>
public struct AppearanceSpriteData
{
    public string Name { get; set; }
    public string State { get; set; }
    public int? Angle { get; set; }
    public int? Index { get; set; }

    public string GetCategoryName() =>
        AppearanceSpriteResolvingUtils.GetCategoryName(Name, State, Angle);
}
