using System.Collections.Generic;
using UnityEngine;

public static class GraphUtils
{
    public static List<Vertex> SmoothPath(List<Vertex> path, float pathWidth = 0.5f)
    {
        List<Vertex> newPath = new List<Vertex>();
        if (path.Count <= 2)
            return path;

        newPath.Add(path[0]);

        int i, j;
        for (i = 0; i < path.Count - 1;)
        {
            for (j = i + 1; j < path.Count; j++)
            {
                Vector2 origin = path[i].transform.position;
                Vector2 destination = path[j].transform.position;

                if (!PathUtils.IsPathClear(origin, destination, pathWidth))
                    break;
            }
            i = j - 1;
            newPath.Add(path[i]);
        }

        return newPath;
    }
}
