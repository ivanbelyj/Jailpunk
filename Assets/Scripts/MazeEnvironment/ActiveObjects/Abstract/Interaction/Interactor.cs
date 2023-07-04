using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RadiusChecker))]
public class Interactor : MonoBehaviour
{
    private RadiusChecker radiusChecker;
    // private HashSet<IInteractable> interactablesInRadius = new HashSet<IInteractable>();
    private void Awake() {
        radiusChecker = GetComponent<RadiusChecker>();
        radiusChecker.NewInRadius += OnNewInRadius;
        radiusChecker.OutOfRadius += OnOutOfRadius;
    }

    public bool IsAvailableToInteract(GameObject gameObject) {
        IInteractable interactable = gameObject.GetComponent<IInteractable>();
        Debug.Log($"in radius: {radiusChecker.IsInRadius(gameObject)}"
            + $" State: {interactable.State}"
            + $" IsObvious: {interactable.IsObvious}");
        return interactable != null
            && radiusChecker.IsInRadius(gameObject)
            && interactable.State == ActivationState.ReadyToActivate
            && interactable.IsObvious;
    }

    private void OnNewInRadius(List<GameObject> newInRadius) {
        ForEachInteractable(newInRadius, interactable => {
            interactable.ActivationStateChanged += OnActivationStateChanged;
            interactable.ObviousnessChanged += OnObviousnessChanged;
            UpdateInteractable(interactable);
        });
    }

    private void OnOutOfRadius(List<GameObject> outOfRadius) {
        ForEachInteractable(outOfRadius, interactable => {
            interactable.ActivationStateChanged -= OnActivationStateChanged;
            interactable.ObviousnessChanged -= OnObviousnessChanged;
            interactable.ClearVisualEffects();
        });
    }

    private void OnObviousnessChanged(IInteractable sender,
        bool old, bool newVal) {
        UpdateInteractable(sender);
    }

    private void OnActivationStateChanged(IActivatable sender,
        ActivationState old, ActivationState newState) {
        IInteractable interactable = (IInteractable)sender;
        UpdateInteractable(interactable);
    }

    private void UpdateInteractable(IInteractable interactable) {
        interactable.SetVisualEffectsForCurrentState();
    }

    private void ForEachInteractable(List<GameObject> gameObjects, Action<IInteractable> action) {
        foreach (var go in gameObjects) {
            var interactable = go.GetComponent<IInteractable>();
            if (interactable != null) {
                action(interactable);
            }
        }
    }
}
