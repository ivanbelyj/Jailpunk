using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    private HashSet<Node<T>> Nodes { get; set; } = new HashSet<Node<T>>();
    public void AddLink(Node<T> node1, Node<T> node2) {
        if (!Nodes.Contains(node1))
            Nodes.Add(node1);
        if (!Nodes.Contains(node2))
            Nodes.Add(node2);
        node1.ConnectedNodes.Add(node2);
        node2.ConnectedNodes.Add(node1);
    }
    public void AddLink(T val1, T val2) {
        AddLink(new Node<T>(val1), new Node<T>(val2));
    }

    public HashSet<(Node<T>, Node<T>)> ConnectedPairsUnique() {
        var addedPairs = new HashSet<(Node<T>, Node<T>)>();
        foreach (Node<T> node in Nodes) {
            foreach (Node<T> linked in node.ConnectedNodes) {
                (Node<T>, Node<T>) pair1 = (node, linked);
                (Node<T>, Node<T>) pair2 = (linked, node);
                if (!addedPairs.Contains(pair1) && !addedPairs.Contains(pair2)) {
                    addedPairs.Add(pair1);
                }
            }
        }
        return addedPairs;
    }
}
