using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(AppearanceBuilder))]
public class CustomizableAppearance : MonoBehaviour, IAppearance
{
    [SerializeField]
    private SpriteLibraryAsset spriteLibraryAsset;

    private SpriteLibraryDecorator spriteLibraryDecorator;

    [SerializeField]
    private AppearanceSchema appearanceSchema;

    [SerializeField]
    private bool setElementsActiveInitially = false;

    [SerializeField]
    private AppearanceData initialAppearanceData;

    private AppearanceBuilder appearanceBuilder;
    private AppearanceElementRenderer[] appearanceElements;
    private AppearanceRenderer appearanceRenderer;

    public AppearanceSchema Schema => appearanceSchema;

    private void Awake() {
        spriteLibraryDecorator = new(spriteLibraryAsset);
        appearanceBuilder = GetComponent<AppearanceBuilder>();
        appearanceBuilder.Initialize(spriteLibraryDecorator);

        appearanceElements = appearanceBuilder.Build(
            appearanceSchema,
            setElementsActiveInitially);

        appearanceRenderer = new(appearanceElements);
        appearanceRenderer.SetAppearance(initialAppearanceData);
    }

    public void Render(AppearanceRenderData appearanceRenderData)
    {
        appearanceRenderer.Render(appearanceRenderData);
    }

    public void SetAppearance(AppearanceData data) {
        appearanceRenderer.SetAppearance(data);
    }

    public void SetElement(string name, AppearanceElementData data) {
        appearanceRenderer.SetElement(name, data);
    }

    // public bool IsOrientationSupported(
    //     AnimationParameters parameters,
    //     int? angle = null)
    // {
    //     return spriteLibraryDecorator.GetCategoryOrNull(
    //         appearanceSchema.mainElementName,
    //         parameters.State,
    //         angle ?? parameters.Angle) != null;
    // }
}
