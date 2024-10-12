using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlashEffect))]
public abstract class InteractableObject : ActivatableObject, IInteractable
{
    private FlashEffect flashEffect;

    [SerializeField]
    private bool isObvious = true;
    public bool IsObvious {
        get => isObvious;
        protected set {
            if (value == isObvious)
                return;
            bool oldObviousness = isObvious;
            isObvious = value;
            ObviousnessChanged?.Invoke(this, oldObviousness, isObvious);
        }
    }

    public event Action<IInteractable, bool, bool> ObviousnessChanged;

    private ActivationState lastStateVisualEffect;

    private void Awake() {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void SetVisualEffectsForCurrentState()
    {
        if (IsObvious && State == ActivationState.ReadyToActivate
            || State == ActivationState.UnableToActivate) {
            Color colorToSet = (State switch {
                ActivationState.UnableToActivate => Color.red,
                _ => Color.white
            });
            Highlight(colorToSet);
        }
    }

    public void ClearVisualEffects()
    {
        ClearHighlight();
    }

    private void Highlight(Color color) {
        flashEffect.Flash(color);
    }

    private void ClearHighlight() {
        flashEffect.FadeOut();
    }
}
