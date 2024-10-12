using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Data related to the maze generation and transferred between
/// generation stages
/// </summary>
public class GenerationContext
{
    /// <summary>
    /// Initial generated sectors rectangles used on early
    /// generation stages
    /// </summary>
    public List<RectArea> Sectors { get; set; }

    /// <summary>
    /// Initial data used to generate corridors between sectors
    /// on early generation stages
    /// </summary>
    public Graph<RectArea> RawCorridorsConnectivity { get; set; }

    /// <summary>
    /// Initial generated root maze corridors used on early
    /// generation stages
    /// </summary>
    public List<CorridorArea> Corridors { get; set; }

    public MazeData MazeData { get; set; }
    public Random Random { get; set; }
}
