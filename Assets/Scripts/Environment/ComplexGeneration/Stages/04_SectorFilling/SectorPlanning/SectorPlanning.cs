using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ISectorPlanningStrategyProvider))]
public class SectorPlanning : GenerationStage
{
    [SerializeField]
    private ISectorPlanningStrategyProvider sectorPlanningStrategyProvider;

    private ApplyAreaHelper applyAreaHelper;

    private void Awake() {
        sectorPlanningStrategyProvider = GetComponent<ISectorPlanningStrategyProvider>();
    }

    public override void RunStage()
    {
        InitializeStage();

        PlanAndApply(context);

        HandleConnectivityAndBoundaries();
    }

    private void InitializeStage() {
        applyAreaHelper = new ApplyAreaHelper(idGenerator);
    }

    private void HandleConnectivityAndBoundaries() {
        var helper = new SectorPlanConnectivityHelper();

        var areaConnectivity = helper.GetAreaConnectivity(context.ComplexData.Scheme);
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
                applyAreaHelper.ApplyToScheme(context.ComplexData.Scheme, schemeArea);
            }
        }
    }

    private List<SchemeArea> PlanAndAdd(
        GenerationContext context,
        GeneratedSectorInfo sector)
    {
        var schemeAreas = PlanSector(sector, context);
        sector.SchemeAreas = schemeAreas;
        context.ComplexData.Scheme.Areas.AddRange(schemeAreas);
        return schemeAreas;
    }

    private List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context)
    {
        var strategy = sectorPlanningStrategyProvider.GetSectorPlanningStrategy(sector);
        return strategy.PlanSector(sector, context);
    }
}
