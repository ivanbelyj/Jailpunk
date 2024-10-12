using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AppearanceRenderData
{
    public string State { get; set; }
    public int Angle { get; set; }
    public int Frame { get; set; }
    public bool FlipX { get; set; }

    /// <summary>
    /// Index of the last frame of 'walk' animation sequence before 
    /// it changed to 'idle'. For states, other than 'idle', null.
    /// </summary>
    public int? LastWalkFrame { get; set; }
}
