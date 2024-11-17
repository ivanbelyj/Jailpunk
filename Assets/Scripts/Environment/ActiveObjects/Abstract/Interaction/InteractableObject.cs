using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlashEffect))]
public abstract class InteractableObject : ActivatableObject, IInteractable
{
    private FlashEffect flashEffect;

    [SerializeField]
    private FlashEffectData defaultEffectData;

    [SerializeField]
    private FlashEffectData unableToActivateEffectData;

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

    private void Awake() {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void SetVisualEffectsForCurrentState()
    {
        if (IsObvious && State == ActivationState.ReadyToActivate
            || State == ActivationState.UnableToActivate) {
            var effectData = State switch {
                ActivationState.UnableToActivate => unableToActivateEffectData,
                _ => defaultEffectData
            };
            Highlight(effectData);
        }
    }

    public void ClearVisualEffects()
    {
        ClearHighlight();
    }

    private void Highlight(FlashEffectData effectData) {
        flashEffect.Flash(effectData);
    }

    private void ClearHighlight() {
        flashEffect.FadeOutLastEffect();
    }
}
