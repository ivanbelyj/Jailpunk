using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AppearanceBuilder))]
public class CustomizableAppearance : MonoBehaviour, IAppearance
{
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

    public event Action<CustomizableAppearance> AppearanceElementsReady;

    public bool IsInitialized { get; private set; }

    private void Start() {
        appearanceBuilder = GetComponent<AppearanceBuilder>();

        var appearanceElementNameResolver = new AppearanceElementNameResolver();
        appearanceElements = appearanceBuilder.Build(
            appearanceSchema,
            appearanceElementNameResolver,
            setElementsActiveInitially);
        AppearanceElementsReady?.Invoke(this);

        appearanceRenderer = new(appearanceElements, appearanceElementNameResolver);

        // TODO: Fix NullReferenceException in Build
        appearanceRenderer.SetAppearance(initialAppearanceData);

        IsInitialized = true;
    }

    public void Render(AppearanceAnimationData appearanceRenderData)
    {
        if (!IsInitialized) {
            Debug.LogError(
                $"{nameof(CustomizableAppearance)} is not initialized. " +
                $"Please, check {nameof(IsInitialized)} before calling {nameof(Render)}");
        }
        // if (appearanceRenderer != null) {
            appearanceRenderer.Render(appearanceRenderData);
        // } else {
        //     Debug.Log("APPEARANCE RENDERER IS NULL");
        // }
    }

    public void SetAppearance(AppearanceData data) {
        appearanceRenderer.SetAppearance(data);
    }

    public void SetElement(string name, AppearanceElementConfig data) {
        appearanceRenderer.SetElement(name, data);
    }

    public GameObject[] GetElements() {
        return appearanceElements.Select(x => x.RendererGameObject).ToArray();
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
