using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class SectorPlanConnectivityHelper
{
    /// <summary>
    /// Gets connectivity of all areas of the scheme
    /// </summary>
    /// <param name="complexScheme"></param>
    /// <returns></returns>
    public ConnectivityResult GetAreaConnectivity(ComplexScheme complexScheme) {
        var connectivityProcessor = new ConnectivityProcessor();
        return connectivityProcessor.ProcessConnectivity(
            (x, y) => {
                return complexScheme.GetTileByPos(x, y).AreaId;
            },
            complexScheme.MapSize);
    }

    public Dictionary<int, Graph<int>> GetSectorAreaConnectivitiesBySectorId(
        List<GeneratedSectorInfo> generatedSectors,
        Graph<int> commonAreaConnectivity)
    {
        var result = new Dictionary<int, Graph<int>>();
        foreach (var areaNode in commonAreaConnectivity.Nodes) {
            var sectorId = FindSectorByAreaId(areaNode.Value, generatedSectors).Id;
            var sectorAreaConnectivity = AddEmptyIfNecessaryAndGet(result, sectorId);
            var pairs = GetPairsFromTheSameSector(
                areaNode.Value,
                areaNode.ConnectedNodes.Select(x => x.Value),
                generatedSectors);
            foreach (var (areaId1, areaId2) in pairs) {
                sectorAreaConnectivity.AddLink(areaId1, areaId2);
            }
        }
        return result;
    }

    private List<(int, int)> GetPairsFromTheSameSector(
        int areaId,
        IEnumerable<int> connectedAreaIds,
        IEnumerable<GeneratedSectorInfo> generatedSectors)
    {
        var sectorByArea = FindSectorByAreaId(areaId, generatedSectors);

        return connectedAreaIds
            .Select(connectedAreaId => {
                var connectedNodeSector = FindSectorByAreaId(
                    connectedAreaId,
                    generatedSectors);

                return connectedNodeSector.Id == sectorByArea.Id
                    ? (areaId, connectedAreaId)
                    : ((int, int)?)null;
            })
            .Where(pair => pair != null)
            .ToList()
            .ConvertAll(x => x.Value);
    }

    private Graph<int> AddEmptyIfNecessaryAndGet(
        Dictionary<int, Graph<int>> connectivitiesBySectorId,
        int sectorId)
    {
        if (!connectivitiesBySectorId.TryGetValue(
            sectorId,
            out var sectorAreaConnectivity))
        {
            sectorAreaConnectivity = new Graph<int>();
            connectivitiesBySectorId.Add(sectorId, sectorAreaConnectivity);
        }
        return sectorAreaConnectivity;
    }

    private GeneratedSectorInfo FindSectorByAreaId(
        int areaId,
        IEnumerable<GeneratedSectorInfo> generatedSectors)
        => generatedSectors.First(x => x.SchemeAreas.Any(x => x.Id == areaId));
}
