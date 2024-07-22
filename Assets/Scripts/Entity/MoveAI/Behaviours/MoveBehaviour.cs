using UnityEngine;

[RequireComponent(typeof(MoveAgent))]
public class MoveBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    public GameObject Target { get => target; set => target = value; }
    protected MoveAgent agent;

    public virtual void Awake() {
        agent = GetComponent<MoveAgent>();
    }
    public virtual void Update() {
        agent.SetSteering(GetSteering());
    }

    public virtual AISteering GetSteering() {
        return new AISteering();
    }
}
