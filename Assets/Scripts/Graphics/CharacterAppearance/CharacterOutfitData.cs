using System;
using UnityEngine;

[Serializable]
public class CharacterOutfitData
{
    public Shirt shirt;
    public Color shirtColor;
    public UpperWear upperWear;
    public Color upperWearColor;
    public LowerWear lowerWear;
    public Color lowerWearColor;

    public AppearanceData ToAppearanceData() {
        return new AppearanceData {
            elements = new AppearanceData.AppearanceDataItem [] {
                CreateBodyPart(
                    shirt switch {
                        _ => "shirt"
                    },
                    shirtColor,
                    shirt != Shirt.None),
                CreateBodyPart(
                    upperWear switch {
                        _ => "upper-wear"
                    },
                    upperWearColor,
                    upperWear != UpperWear.None),
                CreateBodyPart(
                    lowerWear switch {
                        _ => "lower-wear-left"
                    },
                    lowerWearColor,
                    lowerWear != LowerWear.None),
                CreateBodyPart(
                    lowerWear switch {
                        _ => "lower-wear-right"
                    },
                    lowerWearColor,
                    lowerWear != LowerWear.None),
            }
        };
    }

    private AppearanceData.AppearanceDataItem CreateBodyPart(
        string name,
        Color color,
        bool isActive = true) {
        return new () {
            elementName = name,
            elementData = new() {
                color = color,
                isActive = isActive
            }
        };
    }
}
