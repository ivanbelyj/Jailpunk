using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AppearanceBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject appearanceElementPrefab;

    [SerializeField]
    private GameObject spriteMaskPrefab;

    [SerializeField]
    private GameObject spriteMaskRoot;

    private SpriteLibraryDecorator spriteLibraryDecorator;

    public void Initialize(SpriteLibraryDecorator spriteLibraryDecorator) {
        this.spriteLibraryDecorator = spriteLibraryDecorator;
    }

    public AppearanceElementRenderer[] Build(
        AppearanceSchema schema,
        bool setElementsActiveInitially) {
        var res = new AppearanceElementRenderer[schema.Elements.Length];
        for (int i = 0; i < schema.Elements.Length; i++) {
            res[i] = InstantiatePart(schema, schema.Elements[i], setElementsActiveInitially);
        }

        return res;
    }

    private AppearanceElementRenderer InstantiatePart(
        AppearanceSchema appearanceSchema,
        AppearanceElementSchema elementConfig,
        bool setElementsActiveInitially) {
        var result = new AppearanceElementRenderer(
            appearanceSchema,
            elementConfig,
            InstantiateSpriteRenderer(elementConfig),
            InstantiateSpriteMask(elementConfig),
            spriteLibraryDecorator);

        result.SetActive(setElementsActiveInitially);
        return result;
    }

    private SpriteRenderer InstantiateSpriteRenderer(
        AppearanceElementSchema elementConfig
    ) {
        var appearanceElementGO = Instantiate(appearanceElementPrefab, transform, false);
        appearanceElementGO.name = elementConfig.name;

        var spriteRenderer = appearanceElementGO.GetComponent<SpriteRenderer>()
            ?? throw new InvalidOperationException(
                $"Component is required on prefab {nameof(appearanceElementPrefab)} "
                + $"(component name: {nameof(SpriteRenderer)})");

        return spriteRenderer;
    }

    private SpriteMask InstantiateSpriteMask(
        AppearanceElementSchema elementConfig) {
        var spriteMaskGO = Instantiate(spriteMaskPrefab, spriteMaskRoot.transform, false);
        spriteMaskGO.name = elementConfig.name + " Mask";

        var spriteMask = spriteMaskGO.GetComponent<SpriteMask>()
            ?? throw new InvalidOperationException(
                $"Component is required on prefab {nameof(appearanceElementPrefab)} "
                + $"(component name: {nameof(SpriteMask)})");
        
        return spriteMask;
    }
}
