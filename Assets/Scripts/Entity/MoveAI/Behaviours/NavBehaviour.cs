using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavBehaviour : MoveBehaviour
{
    private NavManager navManager;
    public Vector3 CurrentDest { get; set; }
    
    public override void Awake() {
        base.Awake();
        navManager = FindObjectOfType<NavManager>();
    }

    public override AISteering GetSteering()
    {
        Debug.Log($"AStar between: {gameObject.transform.position} and {CurrentDest}");
        List<Vertex> path = GetCurrentPath();
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

        List<Vertex> path = GetCurrentPath();

        if (path.Count > 0)
            DrawPath(path);
        else Debug.Log("Path is empty!");
    }

    private List<Vertex> GetCurrentPath() {
        return navManager.Graph.GetPathAstar(
            gameObject.transform.position,
            CurrentDest);
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