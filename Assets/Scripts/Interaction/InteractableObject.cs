using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlashEffect))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    private FlashEffect flashEffect;

    public InteractionState State {
        get; set;
    }

    private Color lastColorApplied;

    private void Awake() {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void SetVisualEffectsForState(bool val)
    {
        if (val && State == InteractionState.Acceptable
            || State == InteractionState.Unacceptable) {
            Color colorToSet = (State switch {
                InteractionState.Unacceptable => Color.red,
                _ => Color.white
            });
            Highlight(colorToSet);
        } else {
            ClearHighlight();
        }
    }

    private void Highlight(Color color) {
        // If highlight is already applied
        if (lastColorApplied == color) {
            return;
        }
        lastColorApplied = color;
        flashEffect.Flash(color);
    }

    private void ClearHighlight() {
        // If highlight is already faded out
        if (lastColorApplied == default) {
            return;
        }
        lastColorApplied = default;
        flashEffect.FadeOut();
    }
}
