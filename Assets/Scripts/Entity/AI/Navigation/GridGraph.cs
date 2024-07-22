using UnityEngine;
using System;
using System.Collections.Generic;

public class GridGraph : Graph
{
    private readonly float cellSize = 1f;
    private readonly Vector3 gridOrigin;

    private readonly float defaultCost = 1f;
    private readonly float maximumCost = Mathf.Infinity;
    
    public readonly bool[,] mapVertices;

    public int ColsCount => mapVertices.GetLength(1);
    public int RowsCount => mapVertices.GetLength(0);

    public GridGraph(
        float cellSize,
        Vector3 gridOrigin,
        bool[,] mapVertices,
        List<Vertex> vertices,
        List<List<Vertex>> neighbours,
        List<List<float>> costs,
        float? defaultCost = null,
        float? maximumCost = null) : base(vertices, neighbours, costs)
    {
        this.gridOrigin = gridOrigin;
        this.cellSize = cellSize;
        this.mapVertices = mapVertices;
        this.defaultCost = defaultCost ?? this.defaultCost;
        this.maximumCost = maximumCost ?? this.maximumCost;
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        int col = (int)((position.x - gridOrigin.x) / cellSize);
        int row = (int)((position.y - gridOrigin.y) / cellSize);

        Vector2 p = new Vector2(col, row);
        List<Vector2> explored = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(p);
        do
        {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            if (mapVertices[row, col]) {
                return GetVertex(id);
            }
            
            if (!explored.Contains(p))
            {
                explored.Add(p);
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    { 
                        if (i < 0 || j < 0)
                            continue;
                        if (j >= ColsCount || i >= RowsCount)
                            continue;
                        if (i == row && j == col)
                            continue;
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        return null;
    }

    public int GridToId(int x, int y)
    {
        return Math.Max(RowsCount, ColsCount) * y + x;
    }

    // public Vector2 IdToGrid(int id)
    // {
    //     Vector2 location = Vector2.zero;
    //     location.y = Mathf.Floor(id / ColsCount);
    //     location.x = Mathf.Floor(id % ColsCount);
    //     return location;
    // }

    /// <summary>
    /// Sets all neighbours
    /// </summary>
    public void SetNeighbours()
    {
        for (int i = 0; i < RowsCount; i++)
        {
            for (int j = 0; j < ColsCount; j++)
            {
                SetNeighbours(j, i);
            }
        }
    }
    
    private void SetNeighbours(int x, int y, bool get8 = false)
    {
        int col = x;
        int row = y;
        int i, j;
        int vertexId = GridToId(x, y);
        Neighbours[vertexId] = new List<Vertex>();
        Costs[vertexId] = new List<float>();
        Vector2[] pos = new Vector2[0];
        if (get8)
        {
            pos = new Vector2[8];
            int c = 0;
            for (i = row - 1; i <= row + 1; i++)
            {
                for (j = col -1; j <= col; j++)
                {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }       
        }
        else
        {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 1);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);   
        }
        foreach (Vector2 p in pos)
        {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j < 0)
                continue;
            if (i >= RowsCount || j >= ColsCount)
                continue;
            if (i == row && j == col)
                continue;
            if (!mapVertices[i, j])
                continue;
            int id = GridToId(j, i);
            Neighbours[vertexId].Add(Vertices[id]);
            Costs[vertexId].Add(defaultCost);
        }
    }
}
