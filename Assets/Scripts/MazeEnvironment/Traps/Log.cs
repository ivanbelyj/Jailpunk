using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Log : MonoBehaviour, IRefreshable
{
    [SerializeField]
    private float rollForce = 10f;

    [SerializeField]
    private MazeObjectOrientation orientation;

    [SerializeField]
    private Area activatingArea;

    private Rigidbody2D rb;
    public ActivatableState State { get; private set; }
        = ActivatableState.ReadyToActivate;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        activatingArea.OnAreaEntered += (go) => {
            if (State == ActivatableState.ReadyToActivate) {
                Debug.Log("Activating log");
                Activate();

                // Todo: activating
                State = ActivatableState.Activated;
            }
        };
    }

    public void Activate()
    {
        rb.AddForce(rollForce * IsometricUtils
            .MazeObjectOrientationToVector2(orientation), ForceMode2D.Impulse);
    }

    private void OnDrawGizmos() {
        Color prevColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, IsometricUtils
            .MazeObjectOrientationToVector2(orientation));
        Gizmos.color = prevColor;
    }

    public void Refresh()
    {
        throw new System.NotImplementedException();
    }
}
