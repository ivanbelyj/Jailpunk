using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AISeek : AIBehaviour
{
    private Player player;
    private NavManager navManager;

    public override void Awake() {
        base.Awake();
        navManager = FindObjectOfType<NavManager>();
    }

    public override void Update() {
        if (player == null) {
            Debug.Log("Finding player...");
            player = FindObjectOfType<Player>();
        }
        if (player != null)
            base.Update();
    }

    public override AISteering GetSteering()
    {
        Debug.Log($"AStar between: {gameObject.transform.position} and {player.transform.position}");
        List<Vertex> path = navManager.Graph.GetPathAstar(gameObject, player.gameObject);
        Debug.Log("Path: " + string.Join(", ", path.Select(x => x.transform.position).ToList()));
        if (path.Count > 1)
            Target = path[path.Count - 2].gameObject;
        else
            Target = gameObject;

        AISteering steering = new AISteering();
        steering.Linear = Target.transform.position - transform.position;
        steering.Linear.Normalize();
        steering.Linear *= agent.MaxAcceleration;
        return steering;
    }

    private void OnDrawGizmos() {
        if (navManager == null)
            return;

        List<Vertex> path = navManager.Graph.GetPathAstar(gameObject, player.gameObject);
        if (path.Count > 0)
            DrawPath(path);
        else Debug.Log("Path is empty!");
    }

    private void DrawPath(List<Vertex> path)
    {
        if (path == null || path.Count == 0) return;
        Color color = Color.red;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
        }
    }
}
