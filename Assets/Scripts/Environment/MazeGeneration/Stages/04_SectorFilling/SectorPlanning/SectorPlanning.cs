using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorPlanning : GenerationStage
{
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        foreach (var sector in context.GeneratedSectors) {
            PlanSector(sector, context);
        }
        return context;
    }

    private void PlanSector(GeneratedSectorInfo sector, GenerationContext context) {
        var strategy = SelectPlanningStrategy(sector);
        strategy.PlanSector(sector, context);
    }

    private ISectorPlanningStrategy SelectPlanningStrategy(GeneratedSectorInfo sector) {
        return new SectorRoomPlanningStrategy();
    }
}
