using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : ActivatableObject, IInteractable
{
    [SerializeField]
    private bool useFlashEffect = true;

    [SerializeField]
    private List<FlashEffect> flashEffectComponents;
    public List<FlashEffect> FlashEffectComponents {
        get => flashEffectComponents;
        set => flashEffectComponents = value;
    }
    
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

    protected virtual void Awake() {
        
    }

    protected virtual void Start() {
        
    }

    public void SetVisualEffectsForCurrentState()
    {
        if (IsObvious && State == ActivationState.ReadyToActivate
            || State == ActivationState.UnableToActivate) {
            var effectData = State switch {
                ActivationState.UnableToActivate => unableToActivateEffectData,
                _ => defaultEffectData
            };
            if (useFlashEffect) {
                Highlight(effectData);
            }
        }
    }

    public void ClearVisualEffects()
    {
        if (useFlashEffect) {
            ClearHighlight();
        }
    }

    private void Highlight(FlashEffectData effectData) {
        if (flashEffectComponents != null) {
            foreach (var flashEffect in flashEffectComponents) {
                flashEffect.Flash(effectData);
            }
        }
    }

    private void ClearHighlight() {
        if (flashEffectComponents != null) {
            foreach (var flashEffect in flashEffectComponents) {
                flashEffect.FadeOutLastEffect();
            }
        }
    }
}
