using System.Collections.Generic;
using UnityEngine;

public abstract class SectorPlanningStrategyBase : ISectorPlanningStrategy
{
    public int Id { get; set; }

    public abstract List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context);
}
