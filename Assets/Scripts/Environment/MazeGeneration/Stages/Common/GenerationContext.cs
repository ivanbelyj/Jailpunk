using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Data related to the maze generation and transferred between
/// generation stages
/// </summary>
public class GenerationContext
{
    #region Common
    public MazeData MazeData { get; set; }

    // public Random Random { get; set; }

    public string RootGameObjectName { get; set; } = "MazeGrid";

    public GenerationSettings Settings { get; set; }
    public GenerationRequest Request { get; set; }

    public SectorIdGenerator SectorIdGenerator { get; set; } = new();
    #endregion
    
    #region Structure
    /// <summary>
    /// Rectangles of the sectors generated initially
    /// </summary>
    public List<RectArea> SectorRects { get; set; }

    /// <summary>
    /// Initial data used to generate corridors between sectors
    /// on early generation stages
    /// </summary>
    public Graph<RectArea> RawCorridorsConnectivity { get; set; }

    /// <summary>
    /// Initial generated root maze corridors
    /// </summary>
    public List<CorridorArea> Corridors { get; set; }
    #endregion

    /// <summary>
    /// Sector ids that could be connected
    /// </summary>
    public Graph<int> PossibleConnectivity { get; set; }

    public List<AreasBoundary> SectorBoundaries { get; set; } = new List<AreasBoundary>();

    /// <summary>
    /// Actually generated sectors info, including <see cref="RequestedSectors"/>
    /// </summary>
    public List<SectorInfo> GeneratedSectors { get; set; }
}