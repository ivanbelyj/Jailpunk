using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveAgent : MonoBehaviour
{
    [SerializeField]
    private float maxAcceleration;
    public float MaxAcceleration {
        get => maxAcceleration;
        set => maxAcceleration = value;
    }

    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    public Vector2 Velocity { get; private set; } = Vector2.zero;

    protected AISteering steering = new AISteering();
    public void SetSteering(AISteering steering) {
        this.steering = steering;
    }

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate() {
        Vector2 displacement = Velocity * Time.fixedDeltaTime;
        transform.Translate(displacement, Space.World);

        Velocity += steering.Linear * Time.fixedDeltaTime;
        if (Velocity.magnitude > MaxSpeed) {
            Velocity = Velocity.normalized * MaxSpeed;
        }

        if (steering.Linear.sqrMagnitude == 0f) {
            Velocity = Vector2.zero;
        }
        steering = new AISteering();
    }
}
