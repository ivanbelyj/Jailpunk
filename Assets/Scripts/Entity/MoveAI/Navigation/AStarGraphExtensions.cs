using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on https://github.com/PacktPublishing/Unity-5.x-Game-AI-Programming-Cookbook/blob/master/UAIPC/Assets/Scripts/Ch02Navigation/Graph.cs
public static class AStarExtensions
{
    public static List<Vertex> GetPathAstar(
        this Graph graph,
        Vector3 srcPos,
        Vector3 dstPos,
        float pathWidth = 0.5f,
        PathCostHeuristic h = null)
    {
        h ??= PathCostHeuristicUtils.EuclideanEstimate;

        Vertex src = graph.GetNearestVertex(srcPos);
        Vertex dst = graph.GetNearestVertex(dstPos);

        Debug.Log(
            $"Objects: {srcPos} and {dstPos}; "
            + $" Vertices: {src.gameObject.transform.position}"
            + $" and {dst.gameObject.transform.position}");

        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        
        Edge[] edges;
        Edge node, child;
        int size = graph.GetSize();
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.Id] = 0;
        previous[src.Id] = src.Id;
        for (int i = 0; i < size; i++)
        {
            if (i == src.Id)
                continue;
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            node = frontier.Remove();
            int nodeId = node.Vertex.Id;
            if (ReferenceEquals(node.Vertex, dst))
            {                 
                return GraphUtils.SmoothPath(
                    graph.BuildPath(src.Id, node.Vertex.Id, ref previous),
                    pathWidth
                );
            }
            edges = graph.GetEdges(node.Vertex);
            foreach (Edge e in edges)
            {
                int eId = e.Vertex.Id;
                if (previous[eId] != -1)
                    continue;
                float cost = distValue[nodeId] + e.Cost;
                // key point
                cost += h(node.Vertex, e.Vertex);
                if (cost < distValue[e.Vertex.Id])
                {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.Vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }

    private static List<Vertex> BuildPath(
        this Graph graph,
        int srcId,
        int dstId,
        ref int[] prevList)
    {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do
        {
            path.Add(graph.GetVertex(prev));
            prev = prevList[prev];
        } while (prev != srcId);
        return path;
    }
}
