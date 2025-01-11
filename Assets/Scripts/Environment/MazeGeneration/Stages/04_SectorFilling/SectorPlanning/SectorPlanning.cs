using System.Collections.Generic;
using UnityEngine;

public class SectorPlanning : GenerationStage
{
    private ISectorPlanningStrategy[] planningStrategies;
    private ApplyAreaHelper applyAreaHelper;

    public override void ProcessMaze()
    {
        InitializeStage();

        PlanAndApply(context);

        HandleConnectivityAndBoundaries();
    }

    private void InitializeStage() {
        planningStrategies =  new ISectorPlanningStrategy[] {
            new SectorRoomPlanningStrategy(idGenerator),
            // new GladePlanningStrategy(idGenerator)
        };

        applyAreaHelper = new ApplyAreaHelper(idGenerator);
    }

    private void HandleConnectivityAndBoundaries() {
        var helper = new SectorPlanConnectivityHelper();

        var areaConnectivity = helper.GetAreaConnectivity(context.MazeData.Scheme);
        GenerationData.AreaBoundariesAllSectors = areaConnectivity.Boundaries;
        GenerationData.AreaPossibleConnectivityAllSectors = areaConnectivity.ConnectivityGraph;

        GenerationData.AreaPossibleConnectivityBySectorId = helper.GetSectorAreaConnectivitiesBySectorId(
            GenerationData.GeneratedSectors,
            areaConnectivity.ConnectivityGraph);
    }

    private void PlanAndApply(GenerationContext context)
    {
        foreach (var sector in GenerationData.GeneratedSectors) {
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
        return planningStrategies[Random.Range(0, planningStrategies.Length)];
    }
}
