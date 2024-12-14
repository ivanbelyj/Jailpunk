using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteLibraryDecorator
{
    private Dictionary<string, string[]> categoriesAndLabels;

    // TODO: PUBLIC FOR DEBUG
    public SpriteLibraryAsset spriteLibraryAsset;

    public SpriteLibraryDecorator(SpriteLibraryAsset spriteLibraryAsset)
    {
        this.spriteLibraryAsset = spriteLibraryAsset;

        categoriesAndLabels = spriteLibraryAsset
            .GetCategoryNames()
            .ToDictionary(
                x => x,
                x => spriteLibraryAsset.GetCategoryLabelNames(x).ToArray());
    }

    public string GetCategoryOrNull(string categoryName) {
        if (!HasCategory(categoryName)) {
            return null;
        }

        return categoryName;
    }

    public string GetCategoryOrNull(string @base, string state, int? angle) {
        string categoryName = AppearanceSpriteResolvingUtils.GetCategoryName(@base, state, angle);
        return GetCategoryOrNull(categoryName);
    }

    public bool HasCategory(string category) {
        return categoriesAndLabels.ContainsKey(category);
    }

    public string[] GetLabels(string category)
    {
        return categoriesAndLabels[category];
    }

    public int LabelsCount(string categoryName) {
        return categoriesAndLabels[categoryName].Length;
    }

    public Sprite GetSprite(string category, string label) {
        return spriteLibraryAsset.GetSprite(category, label);
    }
}
