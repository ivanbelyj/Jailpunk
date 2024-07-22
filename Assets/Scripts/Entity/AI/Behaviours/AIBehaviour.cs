using UnityEngine;

[RequireComponent(typeof(MovementAgent))]
public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    public GameObject Target { get => target; set => target = value; }
    protected MovementAgent agent;

    public virtual void Awake() {
        agent = GetComponent<MovementAgent>();
    }
    public virtual void Update() {
        agent.SetSteering(GetSteering());
    }

    public virtual AISteering GetSteering() {
        return new AISteering();
    }

    /// <summary>
    /// Возвращает значение поворота в диапазоне [-180f; 180f]
    /// </summary>
    public static float MapOrientationToRange180(float orientation) {
        orientation %= 360f;
        if (Mathf.Abs(orientation) > 180f) {
            if (orientation < 0f) {
                orientation += 360f;
            } else {
                orientation -= 360f;
            }
        }
        return orientation;
    }

    /// <summary>
    /// Преобразование направления в вектор для 2D пространства
    /// </summary>
    public static Vector2 OrientationToVector(float orientation) {
        Vector2 vector = Vector2.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad);
        vector.y = Mathf.Cos(orientation * Mathf.Deg2Rad);
        return vector.normalized;
    }
}
