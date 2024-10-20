using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterBar : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject fill;

    private Transform fillTransform;
    private SpriteRenderer fillSpriteRenderer;
    private SpriteRenderer backgroundSpriteRenderer;

    private float initialScaleX;
    private float initialLocalPosX;

    [SerializeField]
    private bool interpolateColor = true;

    [SerializeField]
    private Color minColor;
    [SerializeField]
    private Color maxColor;

    /// <summary>
    /// Sets parameter value and updates UI
    /// </summary>
    /// <param name="value">
    /// Parameter value in [0, 1]
    /// </param>
    public void SetValue(float value) {
        value = Mathf.Clamp01(value);

        SetVisible(value < 1f);

        Vector3 newPosition = fillTransform.localPosition;
        newPosition.x = initialLocalPosX - (initialScaleX - (initialScaleX * value)) / 2;
        fillTransform.localPosition = newPosition;
        Vector3 newScale = fillTransform.localScale;
        newScale.x = initialScaleX * value;
        fillTransform.localScale = newScale;

        if (interpolateColor)
            fillSpriteRenderer.color = Color.Lerp(minColor, maxColor, value);
    }

    private void SetVisible(bool val) {
        fillSpriteRenderer.enabled = val;
        backgroundSpriteRenderer.enabled = val;
    }

    private void Awake() {
        fillTransform = fill.GetComponent<Transform>();
        fillSpriteRenderer = fill.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();

        initialScaleX = fillTransform.localScale.x;
        initialLocalPosX = fillTransform.localPosition.x;
    }
}
