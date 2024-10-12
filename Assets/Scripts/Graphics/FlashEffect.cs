using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls Sprites/DefaultColorFlash custom shader
/// </summary>
[RequireComponent(typeof(Renderer))]
public class FlashEffect : MonoBehaviour
{
	private new Renderer renderer;
	private bool flashing;

	[SerializeField]
	[Range(0f, 1f)]
	private float maxFlashAmount = 0.3f;

	[SerializeField]
	[Tooltip("The bigger value the bigger speed. When the value equals to 1, "
		+ "the transition time is 1 second")]
	private float flashSpeed = 2;

	private void Awake() {
		renderer = GetComponent<Renderer>();
	}

	private IEnumerator ChangeFlash(float from, float to, float speed) {
		if (flashing)
			yield break;
		flashing = true;
		float t = from;
		while (from > to ? (t > to) : (t < to)) {
			t += (from > to ? -1 : 1) * Time.deltaTime * speed;
			renderer.material.SetFloat("_FlashAmount", t);
			yield return new WaitForEndOfFrame();
		}
	
		flashing = false;
	}

	/// <summary>
	/// Applies flash effect
	/// </summary>
	public void Flash(Color color) {
		renderer.material.SetColor("_FlashColor", color);
		StartCoroutine(ChangeFlash(0f, maxFlashAmount, flashSpeed));
	}

	/// <summary>
	/// Clears flash effect
	/// </summary>
	public void FadeOut() {
		StartCoroutine(ChangeFlash(maxFlashAmount, 0f, flashSpeed));
	}
}
