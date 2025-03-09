using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data defining the sprite to resolve
/// </summary>
[System.Serializable]
public struct AppearanceSpriteData
{
    [SerializeField]
    private string name;

    [SerializeField]
    private string state;

    [SerializeField]
    private bool useAngle;

    [SerializeField]
    private int angle;

    [SerializeField]
    private bool useIndex;

    [SerializeField]
    private int index;

    public string Name { get => name; set => name = value; }
    public string State { get => state; set => state = value; }
    public int? Angle
    {
        get => useAngle ? angle : null;
        set
        {
            if (value == null) {
                useAngle = false;
                angle = 0;
                return;
            }
            useAngle = true;
            angle = value.Value;
        }
    }
    public int? Index
    {
        get => useIndex ? index : null;
        set
        {
            if (value == null) {
                useIndex = false;
                index = 0;
                return;
            }
            useIndex = true;
            index = value.Value;
        }
    }

    public string GetCategoryName() =>
        AppearanceSpriteResolvingUtils.GetCategoryName(Name, State, Angle);

    public override string ToString()
    {
        return AppearanceSpriteResolvingUtils.ToAppearanceDataId(this);
    }
}
