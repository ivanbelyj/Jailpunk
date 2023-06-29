using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Log : MazeObject
{
    [SerializeField]
    private float rollForce = 10f;

    [SerializeField]
    private Area activatingArea;

    private Rigidbody2D rb;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        activatingArea.OnAreaEntered += (go) => {
            if (State == ActivatableState.ReadyToActivate) {
                Activate();
            }
        };
    }

    public override void Activate()
    {
        Debug.Log("Activating log");

        rb.AddForce(rollForce * IsometricUtils
            .MazeObjectOrientationToVector2(orientation), ForceMode2D.Impulse);
        
        // Todo: activating
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
