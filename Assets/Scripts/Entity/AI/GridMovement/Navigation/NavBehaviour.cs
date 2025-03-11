using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NavBehaviour : NavBehaviourCore
{
    new private Collider2D collider2D;

    private float ColliderDiameter => collider2D.bounds.extents.magnitude * 2;
    
    protected override void Awake() {
        base.Awake();
        collider2D = GetComponent<Collider2D>();
    }

    protected override Vector3 GetTargetPosition() {
        // Debug.Log($"AStar between: {gameObject.transform.position} and {CurrentDest}");
        List<Vertex> path = GetCurrentPath();
        // Debug.Log("Path: " + string.Join(", ", path.Select(x => x.transform.position).ToList()));

        if (path.Count <= 1)
            return gameObject.transform.position;

        if (PathUtils.IsPathClear(
            gameObject.transform.position,
            path[1].transform.position,
            ColliderDiameter)) {
            return path[1].gameObject.transform.position;
        } else {
            // If we cannot go to the next point, we should go to the center
            // of our origin
            return path[0].gameObject.transform.position;
        }
    }

    private void OnDrawGizmos() {
        if (navManager == null)
            return;

        List<Vertex> path = GetCurrentPath();

        if (path.Count > 0)
            DrawPath(path);
        // else Debug.Log("Path is empty!");
    }

    private List<Vertex> GetCurrentPath() {
        return GraphUtils.SmoothPath(
            navManager.Graph.GetPathAstar(
                gameObject.transform.position,
                CurrentDestination),
            ColliderDiameter);
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
