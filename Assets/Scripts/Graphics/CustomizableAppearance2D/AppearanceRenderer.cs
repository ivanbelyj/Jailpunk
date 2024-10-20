using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppearanceRenderer
{
    private readonly AppearanceElementRenderer[] elements;
    private readonly Dictionary<string, AppearanceElementRenderer> elementsByName;

    [SerializeField]
    private bool showWarnings = false;

    public AppearanceRenderer(AppearanceElementRenderer[] elements)
    {
        this.elements = elements;
        elementsByName = elements.ToDictionary(x => x.Name, x => x);
    }

    public void Render(AppearanceRenderData appearanceRenderData)
    {
        foreach (var element in elements) {
            element.Render(appearanceRenderData);
        }
    }

    public void SetAppearance(AppearanceData appearanceData) {
        foreach (var dataItem in appearanceData.elements) {
            SetElement(dataItem.elementName, dataItem.elementData);
        }
    }

    public void SetElement(string elementName, AppearanceElementData data) {
        if (!elementsByName.TryGetValue(elementName, out var element)) {
            if (showWarnings) {
                Debug.LogWarning(
                    $"Cannot set appearance element '{elementName}'. Not found.");
            }
            return;
        }
        
        element.Apply(data);
    }
}
