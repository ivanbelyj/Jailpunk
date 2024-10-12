using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteLibraryDecorator
{
    private Dictionary<string, string[]> categoriesAndLabels;
    private SpriteLibraryAsset spriteLibraryAsset;

    public SpriteLibraryDecorator(SpriteLibraryAsset spriteLibraryAsset)
    {
        this.spriteLibraryAsset = spriteLibraryAsset;

        categoriesAndLabels = new Dictionary<string, string[]>();
        foreach (string category in spriteLibraryAsset.GetCategoryNames())
        {
            categoriesAndLabels.Add(
                category,
                spriteLibraryAsset.GetCategoryLabelNames(category).ToArray());
        }
    }

    public string GetCategoryOrNull(string categoryName) {
        if (!HasCategory(categoryName)) {
            return null;
        }

        return categoryName;
    }

    public string GetCategoryOrNull(string @base, string state, int? angle) {
        string categoryName = SpriteLibraryUtils.GetCategoryName(@base, state, angle);
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
