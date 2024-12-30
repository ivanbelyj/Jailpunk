using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladePlanningStrategy : ISectorPlanningStrategy
{
    public List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context)
    {
        return new List<SchemeArea> {
            new() {
                Id = context.IdGenerator.NewAreaId(),
                Rect = sector.RectArea.Rect,
                Type = SchemeAreaType.Room
            }
        };
    }
}
