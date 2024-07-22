using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : IComparable<Edge>
{
    public Vertex Vertex { get; set; }
    public float Cost { get; set; }

    public Edge(Vertex vertex, float cost)
    {
        Vertex = vertex;
        Cost = cost;
    }

    public int CompareTo(Edge other)
    {
        if (ReferenceEquals(Vertex, other.Vertex))
            return 0;
        
        float res = Cost - other.Cost;
        return (int)res;
    }

    public bool Equals(Edge other) {
        return Vertex.Id == other.Vertex.Id;
    }

    public override bool Equals(object other) {
        if (other is Edge) 
            return Equals(other);

        return false;
    }
    public override int GetHashCode()
    {
        return Vertex.GetHashCode();
    }
}
