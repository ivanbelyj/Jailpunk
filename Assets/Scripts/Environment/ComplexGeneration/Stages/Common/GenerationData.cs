using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// Initial generated root complex corridors
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
    /// Actually generated sectors info
    /// </summary>
    public List<GeneratedSectorInfo> GeneratedSectors { get; set; }

    public Dictionary<int, int> GeneratedSectorIdsByAllocationRequestId { get; set; }

    public Dictionary<int, Dictionary<int, int>> GeneratedAreaIdsByAllocationRequestIdBySectorRequestId { get; set; } = new();

    #region Scene
    public InstantiatedComplexData InstantiatedComplexData { get; set; }
    #endregion

    // Todo: move to extension class
    public int? GetGeneratedSectorId(SectorRequest sectorRequest) {
        var key = sectorRequest.AreaAllocationRequest.Id;
        GeneratedSectorIdsByAllocationRequestId.TryGetValue(key, out var result);
        return result;
    }
    
    public int? GetGeneratedZoneId(ZoneRequest zoneRequest, int sectorRequestId) {
        var key = zoneRequest.AreaAllocationRequest.Id;
        var generatedZoneIdsByAllocationRequestId =
            GeneratedAreaIdsByAllocationRequestIdBySectorRequestId[sectorRequestId];
        generatedZoneIdsByAllocationRequestId.TryGetValue(key, out var result);
        return result;
    }
}
