using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GridOriented))]
public class Log : ActivatableObject
{
    [SerializeField]
    private float rollForce = 10f;

    [SerializeField]
    private Area activatingArea;

    private Rigidbody2D rb;
    private GridOriented gridOriented;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        gridOriented = GetComponent<GridOriented>();
        activatingArea.OnAreaEntered += (go) => {
            if (State == ActivatableState.ReadyToActivate) {
                Activate();
            }
        };
    }

    public override void Activate()
    {
        rb.AddForce(rollForce * gridOriented.Forward, ForceMode2D.Impulse);
        State = ActivatableState.Activating;
    }

    private void Update() {
        if (State == ActivatableState.Activating) {
            if (rb.velocity.sqrMagnitude < Mathf.Epsilon) {
                State = ActivatableState.Activated;
            }
        }
    }
}
