using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GridOriented))]
public class Log : ActivatableObject
{
    [SerializeField]
    private float rollForce = 10f;

    [SerializeField]
    private Area activatingArea;

    private new Collider2D collider2D;
    private Rigidbody2D rb;
    private GridOriented gridOriented;
    
    private void Awake() {
        collider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        gridOriented = GetComponent<GridOriented>();
        activatingArea.AreaEntered += (go) => {
            // Temporary solution
            if (go.GetComponent<PlayerControls>() != null &&
                State == ActivationState.ReadyToActivate) {
                Activate();
            }
        };
    }

    protected override void Activate()
    {
        rb.AddForce(rollForce * gridOriented.Forward, ForceMode2D.Impulse);
        State = ActivationState.Activating;
    }

    private void Update() {
        if (State == ActivationState.Activating) {
            if (rb.velocity.sqrMagnitude < Mathf.Epsilon) {
                State = ActivationState.UnableToActivate;
            }
        }
    }
}
