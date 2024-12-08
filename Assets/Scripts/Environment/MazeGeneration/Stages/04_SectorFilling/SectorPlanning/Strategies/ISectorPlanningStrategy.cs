using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISectorPlanningStrategy
{
    void PlanSector(SectorInfo sector, GenerationContext context);
}
