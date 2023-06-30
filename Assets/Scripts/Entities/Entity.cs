using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityLifecycle))]
public class Entity : MonoBehaviour
{
    [SerializeField]
    private ParameterBar healthBar;
    private EntityLifecycle lifecycle;

    private void Awake() {
        lifecycle = GetComponent<EntityLifecycle>();
        lifecycle.Health.OnValueChanged += (oldVal, newVal) => {
            UpdateHealthBar(newVal);
        };
    }

    private void Start() {
        UpdateHealthBar(lifecycle.Health.Value);   
    }

    private void UpdateHealthBar(float newVal) {
        healthBar.SetValue(newVal);
    }
}
