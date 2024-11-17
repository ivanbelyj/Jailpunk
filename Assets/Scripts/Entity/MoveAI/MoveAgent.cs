using Mirror;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IMoveControls))]
public class MoveAgent : NetworkBehaviour
{
    // [SerializeField]
    // private float maxAcceleration;
    // public float MaxAcceleration {
    //     get => maxAcceleration;
    //     set => maxAcceleration = value;
    // }

    // [SerializeField]
    // private float maxSpeed;
    // public float MaxSpeed {
    //     get => maxSpeed;
    //     set => maxSpeed = value;
    // }

    // public Vector2 Velocity { get; private set; } = Vector2.zero;

    protected IMoveControls moveControls;

    [SyncVar]
    private AISteering steering = new AISteering();
    
    [Server]
    public void SetSteering(AISteering steering) {
        this.steering = steering;
    }

    // private Rigidbody2D rb;

    private void Awake() {
        // rb = GetComponent<Rigidbody2D>();
        moveControls = GetComponent<IMoveControls>();
    }

    public virtual void Update() {
        moveControls.Move(steering.Linear.normalized);
    }

    // public virtual void FixedUpdate() {
    //     Vector2 displacement = Velocity * Time.fixedDeltaTime;
    //     transform.Translate(displacement, Space.World);

    //     Velocity += steering.Linear * Time.fixedDeltaTime;
    //     if (Velocity.magnitude > MaxSpeed) {
    //         Velocity = Velocity.normalized * MaxSpeed;
    //     }

    //     if (steering.Linear.sqrMagnitude == 0f) {
    //         Velocity = Vector2.zero;
    //     }
    //     steering = new AISteering();
    // }
}
