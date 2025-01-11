using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generation intermediate data, created, processed and used by different
/// generation stages, related to the structure of the generated complex
/// </summary>
public class GenerationData
{
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

    #region Connectivity
    /// <summary>
    /// Sector ids that could be connected
    /// </summary>
    public Graph<int> SectorPossibleConnectivity { get; set; }
    public List<AreasBoundary> SectorBoundaries { get; set; } = new List<AreasBoundary>();

    public Graph<int> AreaPossibleConnectivityAllSectors { get; set; }
    public List<AreasBoundary> AreaBoundariesAllSectors { get; set; } = new List<AreasBoundary>();

    public Dictionary<int, Graph<int>> AreaPossibleConnectivityBySectorId { get; set; }
    #endregion

    /// <summary>
    /// Actually generated sectors info, including at least required requested sectors
    /// </summary>
    public List<GeneratedSectorInfo> GeneratedSectors { get; set; }
}
