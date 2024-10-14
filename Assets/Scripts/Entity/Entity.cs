using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Living being with its lifecycle and HealthBar
/// </summary>
[RequireComponent(typeof(DestroyableLifecycle))]
public class Entity : MonoBehaviour
{
    [SerializeField]
    private ParameterBar healthBar;
    private DestroyableLifecycle lifecycle;

    private void Awake() {
        lifecycle = GetComponent<DestroyableLifecycle>();
        lifecycle.OnEntityDestroyed += OnDeath;
    }

    private void Update() {
        UpdateHealthBar();
    }

    private void OnDeath() {
        Debug.Log("Character died.");
    }

    private void UpdateHealthBar() {
        healthBar.SetValue(lifecycle.GetParameterValue(LifecycleParameterIds.Strength));
    }
}
