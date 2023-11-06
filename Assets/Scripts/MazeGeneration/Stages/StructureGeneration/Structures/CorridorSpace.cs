using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Intermediate representation of the space on the basis of which
/// the corridors will be created.
/// Used on structure generation stage
/// </summary>
public class CorridorSpace
{
    public RectInt Rect { get; set; }
    public bool IsHorizontal { get; set; }
}
