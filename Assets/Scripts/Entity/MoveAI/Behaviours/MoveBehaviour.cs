using Mirror;
using UnityEngine;

[RequireComponent(typeof(MoveAgent))]
public class MoveBehaviour : NetworkBehaviour
{
    [SerializeField]
    private GameObject target;
    public GameObject Target { get => target; set => target = value; }
    protected MoveAgent agent;

    public virtual void Awake() {
        agent = GetComponent<MoveAgent>();
    }

    protected virtual void Update() {
        if (isServer) {
            agent.SetSteering(GetSteering());
        }
    }

    [Server]
    protected virtual AISteering GetSteering() {
        return new AISteering();
    }
}
