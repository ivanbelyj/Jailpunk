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
            AddLinksForGivenSectorOnly(areaNode, generatedSectors, result);
        }
        return result;
    }

    private void AddLinksForGivenSectorOnly(
        Node<int> areaNode,
        IEnumerable<GeneratedSectorInfo> generatedSectors,
        Dictionary<int, Graph<int>> result
    ) {
        var sectorByArea = FindSectorByAreaId(
            generatedSectors,
            areaNode.Value);

        var sectorAreaConnectivity = AddEmptyIfNecessaryAndGet(
            result,
            sectorByArea.Id
        );

        foreach (var connectedNode in areaNode.ConnectedNodes) {
            var connectedNodeSector = FindSectorByAreaId(
                generatedSectors,
                connectedNode.Value);

            if (connectedNodeSector.Id == sectorByArea.Id) {
                sectorAreaConnectivity.AddLink(areaNode.Value, connectedNode.Value);
            }
        }
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
        IEnumerable<GeneratedSectorInfo> generatedSectors,
        int areaId) =>
        generatedSectors.First(x => x.SchemeAreas.Any(x => x.Id == areaId));
}
