using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladePlanningStrategy : SectorPlanningStrategyBase
{
    private readonly IdGenerator idGenerator;

    public GladePlanningStrategy(IdGenerator idGenerator)
    {
        this.idGenerator = idGenerator;
    }

    public override List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context)
    {
        return new List<SchemeArea> {
            new() {
                Id = idGenerator.NewAreaId(),
                Rect = sector.RectArea.Rect,
                Type = SchemeAreaType.CoreArea
            }
        };
    }
}
