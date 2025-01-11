using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Data related to the maze generation and transferred between
/// generation stages
/// </summary>
public class GenerationContext
{
    public MazeData MazeData { get; set; }
    public GenerationData GenerationData => MazeData.GenerationData;
    public string RootGameObjectName { get; set; } = "MazeGrid";

    public GenerationSettings Settings { get; set; }
    public GenerationRequest Request { get; set; }
}
