using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlashEffectData {
	[Range(0f, 1f)]
	public float maxFlashAmount = 0.3f;

	public float flashSpeed = 2f;
	public Color flashColor;
	public Color tint;
}

/// <summary>
/// Script that controls Sprites/DefaultColorFlash custom shader
/// </summary>
[RequireComponent(typeof(Renderer))]
public class FlashEffect : MonoBehaviour
{
	private new Renderer renderer;
	private bool isFlashing;

	private FlashEffectData lastEffectData;

	private void Awake() {
		renderer = GetComponent<Renderer>();
	}

	/// <summary>
	/// Applies flash effect
	/// </summary>
	public void Flash(FlashEffectData effectData) {
		lastEffectData = effectData;
		
		SetEffect(effectData);
		StartCoroutine(ChangeFlash(0f, effectData.maxFlashAmount, effectData.flashSpeed));
	}

	/// <summary>
	/// Clears flash effect
	/// </summary>
	public void FadeOutLastEffect() {
		if (lastEffectData == null) {
			Debug.LogWarning(
				"Cannot fade out the last effect: there is no " +
				"previous effect applied");
			return;
		}
		StartCoroutine(
			ChangeFlash(
				lastEffectData.maxFlashAmount,
				0f,
				lastEffectData.flashSpeed));
	}

	/// <summary>
    /// Applies flash effect and then fades out
    /// </summary>
    public void FlashAndFade(FlashEffectData effectData) {
        SetEffect(effectData);
        StartCoroutine(FlashAndFadeCoroutine(effectData));
    }

    private IEnumerator FlashAndFadeCoroutine(FlashEffectData effectData) {
        yield return ChangeFlash(0f, effectData.maxFlashAmount, effectData.flashSpeed);
        yield return ChangeFlash(effectData.maxFlashAmount, 0f, effectData.flashSpeed);
    }

	private void SetEffect(FlashEffectData effectData) {
		renderer.material.SetColor("_FlashColor", effectData.flashColor);
		renderer.material.SetColor("_Tint", effectData.tint);
	}

	private IEnumerator ChangeFlash(float from, float to, float speed) {
		if (isFlashing)
			yield break;
		isFlashing = true;
		float t = from;
		while (from > to ? (t > to) : (t < to)) {
			t += (from > to ? -1 : 1) * Time.deltaTime * speed;
			renderer.material.SetFloat("_FlashAmount", t);
			yield return new WaitForEndOfFrame();
		}
	
		isFlashing = false;
	}
}
