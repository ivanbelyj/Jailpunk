using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IsometricOrientedObject))]
public class Log : ActivatableObject
{
    [SerializeField]
    private float rollForce = 10f;

    [SerializeField]
    private Area activatingArea;

    private Rigidbody2D rb;
    private IsometricOrientedObject isometricOriented;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        isometricOriented = GetComponent<IsometricOrientedObject>();
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
            .MazeObjectOrientationToVector2(isometricOriented.Orientation),
            ForceMode2D.Impulse);
        
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
