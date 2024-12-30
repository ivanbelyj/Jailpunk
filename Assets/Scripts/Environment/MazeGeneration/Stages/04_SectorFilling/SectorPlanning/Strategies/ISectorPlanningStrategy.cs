using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISectorPlanningStrategy
{
    List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context);
}
