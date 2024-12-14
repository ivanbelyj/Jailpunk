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

    private void Start() {
        appearanceBuilder = GetComponent<AppearanceBuilder>();

        appearanceElements = appearanceBuilder.Build(
            appearanceSchema,
            setElementsActiveInitially);

        appearanceRenderer = new(appearanceElements);
        appearanceRenderer.SetAppearance(initialAppearanceData);
    }

    public void Render(AppearanceAnimationData appearanceRenderData)
    {
        appearanceRenderer.Render(appearanceRenderData);
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
