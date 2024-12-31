using System.Collections.Generic;
using System.Linq;

public class SectorPlanning : GenerationStage
{
    public override void ProcessMaze()
    {
        var applyAreaHelper = new ApplyAreaHelper(context.IdGenerator);

        PlanAndApply(context, applyAreaHelper);

        HandleConnectivityAndBoundaries();
    }

    private void HandleConnectivityAndBoundaries() {
        var helper = new SectorPlanConnectivityHelper();

        var areaConnectivity = helper.GetAreaConnectivity(context.MazeData.Scheme);
        context.AreaBoundariesAllSectors = areaConnectivity.Boundaries;
        context.AreaPossibleConnectivityAllSectors = areaConnectivity.ConnectivityGraph;

        context.AreaPossibleConnectivityBySectorId = helper.GetSectorAreaConnectivitiesBySectorId(
            context.GeneratedSectors,
            areaConnectivity.ConnectivityGraph);
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
