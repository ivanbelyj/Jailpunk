using System;
using UnityEngine;

public class AppearanceBuilder : MonoBehaviour
{
    const string DisableEditorSpriteRendererTooltip =
        "It can be convenient to display appearance in the editor using " + nameof(SpriteRenderer) +
        " and disable it in runtime";

    [SerializeField]
    private GameObject appearanceElementPrefab;

    [SerializeField]
    private GameObject spriteMaskPrefab;

    [SerializeField]
    private GameObject spriteMaskRoot;

    [Header("Editor sprite renderer disabling")]
    [Tooltip(DisableEditorSpriteRendererTooltip)]
    [SerializeField]
    private bool disableEditorSpriteRenderer = true;

    [Tooltip(DisableEditorSpriteRendererTooltip)]
    [SerializeField]
    private SpriteRenderer editorSpriteRendererToDisable;

    private AppearanceSpriteResolver appearanceSpriteResolver;

    private void Awake()
    {
        appearanceSpriteResolver = FindAnyObjectByType<AppearanceSpriteResolver>();
        HandleEditorSpriteRendererDisabling();
    }

    public AppearanceElementRenderer[] Build(
        AppearanceSchema schema,
        IAppearanceElementNameResolver appearanceElementNameResolver,
        bool setElementsActiveInitially)
    {
        var res = new AppearanceElementRenderer[schema.Elements.Length];
        for (int i = 0; i < schema.Elements.Length; i++) {
            res[i] = InstantiatePart(
                schema,
                schema.Elements[i],
                appearanceElementNameResolver,
                setElementsActiveInitially);
        }

        return res;
    }

    private void HandleEditorSpriteRendererDisabling() {
        if (disableEditorSpriteRenderer)
        {
            if (editorSpriteRendererToDisable == null)
            {
                editorSpriteRendererToDisable = GetComponent<SpriteRenderer>();
            }
            if (editorSpriteRendererToDisable != null)
            {
                editorSpriteRendererToDisable.enabled = false;    
            }
        }
    }

    private AppearanceElementRenderer InstantiatePart(
        AppearanceSchema appearanceSchema,
        AppearanceElementSchema elementConfig,
        IAppearanceElementNameResolver appearanceElementNameResolver,
        bool setElementsActiveInitially)
    {
        var result = new AppearanceElementRenderer(
            appearanceSchema,
            elementConfig,
            InstantiateSpriteRenderer(elementConfig),
            InstantiateSpriteMask(elementConfig),
            appearanceSpriteResolver,
            appearanceElementNameResolver);

        result.SetActive(setElementsActiveInitially);
        return result;
    }

    private SpriteRenderer InstantiateSpriteRenderer(
        AppearanceElementSchema elementConfig)
    {
        var appearanceElementGO = Instantiate(appearanceElementPrefab, transform, false);
        appearanceElementGO.name = elementConfig.name;

        var spriteRenderer = appearanceElementGO.GetComponent<SpriteRenderer>()
            ?? throw new InvalidOperationException(
                $"Component is required on prefab {nameof(appearanceElementPrefab)} "
                + $"(component name: {nameof(SpriteRenderer)})");

        return spriteRenderer;
    }

    private SpriteMask InstantiateSpriteMask(
        AppearanceElementSchema elementConfig)
    {
        var spriteMaskGO = Instantiate(spriteMaskPrefab, spriteMaskRoot.transform, false);
        spriteMaskGO.name = elementConfig.name + " Mask";

        var spriteMask = spriteMaskGO.GetComponent<SpriteMask>()
            ?? throw new InvalidOperationException(
                $"Component is required on prefab {nameof(appearanceElementPrefab)} "
                + $"(component name: {nameof(SpriteMask)})");
        
        return spriteMask;
    }
}
