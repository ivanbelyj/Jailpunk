using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float PathCostHeuristic(Vertex a, Vertex b);

public abstract class Graph
{
    protected List<Vertex> Vertices { get; set; }
    protected List<List<Vertex>> Neighbours { get; set; }
    protected List<List<float>> Costs { get; set; }

    public Graph(
        List<Vertex> vertices,
        List<List<Vertex>> neighbours,
        List<List<float>> costs)
    {
        Vertices = vertices;
        Neighbours = neighbours;
        Costs = costs;
    }

    public virtual int GetSize()
    {
        return Vertices?.Count ?? 0;
    }

    public abstract Vertex GetNearestVertex(Vector3 position);

    public virtual Vertex GetVertex(int id) {
        if (Vertices == null
            || Vertices.Count == 0
            || id < 0
            || id >= Vertices.Count)
            return null;
        
        return Vertices[id];
    }

    public virtual Vertex[] GetNeighbours(Vertex v) 
        => GetNeighbours(v.Id);

    public virtual Vertex[] GetNeighbours(int id) {
        if (Neighbours == null
            || Neighbours.Count == 0
            || id < 0
            || id >= Vertices.Count)
            return new Vertex[0];
        
        return Neighbours[id].ToArray();
    }

    public virtual Edge[] GetEdges(Vertex v)
    {
        if (Neighbours == null || Neighbours.Count == 0)
            return new Edge[0];
        if (v.Id < 0 || v.Id >= Neighbours.Count)
            return new Edge[0];

        int numEdges = Neighbours[v.Id].Count;
        Edge[] edges = new Edge[numEdges];
        List<Vertex> vertexList = Neighbours[v.Id];
        List<float> costList = Costs[v.Id];
        for (int i = 0; i < numEdges; i++)
        {
            edges[i] = new Edge(vertexList[i], costList[i]);
        }
        
        return edges;
    }
}
