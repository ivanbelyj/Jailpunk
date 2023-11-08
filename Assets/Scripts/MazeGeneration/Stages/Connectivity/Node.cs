using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public HashSet<Node<T>> ConnectedNodes { get; set; } = new HashSet<Node<T>>();
    public T Value { get; set; }

    public Node(T value) {
        Value = value;
    }
}
