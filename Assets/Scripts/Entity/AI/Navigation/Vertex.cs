using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vertex : MonoBehaviour // Todo: not monobehaviour ?
{
    public int Id { get; set; }
    public List<Edge> Neighbours { get; set; }
    public Vertex Prev { get; set; }
}
