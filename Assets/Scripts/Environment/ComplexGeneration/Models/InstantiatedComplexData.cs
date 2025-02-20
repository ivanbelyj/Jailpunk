using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Data related to instatiated complex
/// </summary>
public class InstantiatedComplexData
{
    public GameObject GridGameObject { get; set; }
    public Tilemap VisionObstacleTilemap { get; set; }
    public Tilemap ObstacleTilemap { get; set; }
}
