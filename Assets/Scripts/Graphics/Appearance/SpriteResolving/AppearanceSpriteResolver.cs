using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public enum AppearanceLibraryItemArrangementType {
    OneObjectWithName,
    NameInCategory
}

[Serializable]
public class AppearanceLibraryItem {
    public AppearanceLibraryItemArrangementType arrangementType;
    public string appearanceName;
    public SpriteLibraryAsset spriteLibraryAsset;
}

// <LibraryName>[_<LibraryVariant>] <object-name>[_<state>][_<angle>] -> Sprite
public class AppearanceSpriteResolver : MonoBehaviour
{
    [SerializeField]
    private List<AppearanceLibraryItem> appearanceLibraryItems;

    // Static to log every not existing category only once
    // regardless of the count of element renderers
    private static readonly HashSet<string> loggedWarningCategories = new();

    private Dictionary<string, SpriteLibraryDecorator> spriteLibrariesByName;

    private List<SpriteLibraryAsset> spriteLibrariesWithNameInCategory;

    private void Awake() {
        SetInitiallySegregatedSpriteLibraryCollections();
    }

    public Sprite Resolve(AppearanceSpriteData data) {
        var spriteLibraryDecorator = GetSpriteLibrary(data);
        if (spriteLibraryDecorator == null) {
            return null;
        }

        var categoryAndLabel = TryGetCurrentCategoryAndLabel(
            spriteLibraryDecorator,
            data);
        if (categoryAndLabel == null) {
            // Debug.LogWarning($"Current category and label not found: {data.GetCategoryName()}");
            return null;
        }

        return spriteLibraryDecorator.GetSprite(
            categoryAndLabel.Value.category,
            categoryAndLabel.Value.label);
    }

    private void SetInitiallySegregatedSpriteLibraryCollections() {
        spriteLibrariesByName = appearanceLibraryItems
            .Where(x => x.arrangementType == AppearanceLibraryItemArrangementType.OneObjectWithName)
            .ToDictionary(
                x => x.appearanceName,
                x => new SpriteLibraryDecorator(x.spriteLibraryAsset));

        spriteLibrariesWithNameInCategory = appearanceLibraryItems
            .Where(x => x.arrangementType == AppearanceLibraryItemArrangementType.NameInCategory)
            .Select(x => x.spriteLibraryAsset)
            .ToList();
    }

    private SpriteLibraryDecorator GetSpriteLibrary(AppearanceSpriteData data) {
        SpriteLibraryDecorator spriteLibrary;
        if (!spriteLibrariesByName.TryGetValue(
            data.Name,
            out spriteLibrary))
        {
            return GetAndCacheSpriteLibraryWithNameInCategory(data);
        }
        return spriteLibrary;
    }

    private SpriteLibraryDecorator GetAndCacheSpriteLibraryWithNameInCategory(
        AppearanceSpriteData data)
    {
        var spriteLibraryAsset = FindSpriteLibraryWithAppearance(data.Name);
        if (spriteLibraryAsset == null) {
            return null;
        }
        var spriteLibrary = new SpriteLibraryDecorator(spriteLibraryAsset);
        spriteLibrariesByName.Add(data.Name, spriteLibrary);

        return spriteLibrary;
    }

    private SpriteLibraryAsset FindSpriteLibraryWithAppearance(string name) {
        return spriteLibrariesWithNameInCategory
            .FirstOrDefault(x => 
                x.GetCategoryNames().Any(x => x.StartsWith(name)));
    }

    private (string category, string label)? TryGetCurrentCategoryAndLabel(
        SpriteLibraryDecorator spriteLibraryDecorator,
        AppearanceSpriteData data)
    {
        var frame = data.Index ?? 0;

        if (frame < 0) {
            return null;
        }

        string categoryName = data.GetCategoryName();

        string category = spriteLibraryDecorator.GetCategoryOrNull(categoryName);

        if (category == null) {
            // if (!loggedWarningCategories.Contains(categoryName)) {
            //     Debug.LogWarning($"Cannot render appearance element: "
            //         + $"category {categoryName} doesn't exist");
            //     loggedWarningCategories.Add(categoryName);
            // }

            return null;
        }
        
        string[] labels = spriteLibraryDecorator.GetLabels(category);

        if (labels.Length == 0) {
            throw new InvalidOperationException(
                "Category " + category + " doesn't contain any labels");
        }

        return (category, GetLabel(frame, labels));
    }

    private string GetLabel(int frame, string[] labels) {
        return labels[frame % labels.Length];
    }
}
