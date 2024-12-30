using System.Collections.Generic;
using System.Linq;

public class SectorPlanning : GenerationStage
{
    public override void ProcessMaze()
    {
        var applyAreaHelper = new ApplyAreaHelper(context.IdGenerator);

        PlanAndApply(context, applyAreaHelper);

        var areaConnectivity = GetAreaConnectivity(context.MazeData.Scheme);
        context.FullAreaBoundaries = areaConnectivity.Boundaries;
        context.FullAreaPossibleConnectivity = areaConnectivity.ConnectivityGraph;

        context.AreaPossibleConnectivityBySectorId = GetSectorAreaConnectivitiesBySectorId(
            context.GeneratedSectors,
            areaConnectivity.ConnectivityGraph);
    }

    private Dictionary<int, Graph<int>> GetSectorAreaConnectivitiesBySectorId(
        List<GeneratedSectorInfo> generatedSectors,
        Graph<int> commonAreaConnectivity)
    {
        // TODO: Refactor and optimize
        var result = new Dictionary<int, Graph<int>>();
        foreach (var areaNode in commonAreaConnectivity.Nodes) {
            var nodeSector = generatedSectors.First(
                sector => sector.SchemeAreas.Any(area => areaNode.Value == area.Id));
            if (!result.TryGetValue(nodeSector.Id, out var sectorAreaConnectivity))
            {
                sectorAreaConnectivity = new Graph<int>();
                result.Add(nodeSector.Id, sectorAreaConnectivity);
            }
            foreach (var connectedNode in areaNode.ConnectedNodes) {
                var connectedNodeSector = generatedSectors.First(x => x.SchemeAreas.Any(x => x.Id == areaNode.Value));
                if (connectedNodeSector.Id == nodeSector.Id) {
                    sectorAreaConnectivity.AddLink(areaNode.Value, connectedNode.Value);
                }
            }
        }
        return result;
    }

    private void PlanAndApply(
        GenerationContext context,
        ApplyAreaHelper applyAreaHelper)
    {
        foreach (var sector in context.GeneratedSectors) {
            foreach (var schemeArea in PlanAndAdd(context, sector)) {
                applyAreaHelper.ApplyToScheme(context.MazeData.Scheme, schemeArea);
            }
        }
    }

    private ConnectivityResult GetAreaConnectivity(MazeScheme mazeScheme) {
        var connectivityProcessor = new ConnectivityProcessor();
        return connectivityProcessor.ProcessConnectivity(
            (x, y) => {
                return mazeScheme.GetTileByPos(x, y).AreaId;
            },
            mazeScheme.MapSize);
    }

    private List<SchemeArea> PlanAndAdd(
        GenerationContext context,
        GeneratedSectorInfo sector)
    {
        var schemeAreas = PlanSector(sector, context);
        sector.SchemeAreas = schemeAreas;
        context.MazeData.Scheme.Areas.AddRange(schemeAreas);
        return schemeAreas;
    }

    private List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context)
    {
        var strategy = SelectPlanningStrategy(sector);
        return strategy.PlanSector(sector, context);
    }

    private ISectorPlanningStrategy SelectPlanningStrategy(GeneratedSectorInfo sector)
    {
        return new SectorRoomPlanningStrategy();
    }
}
