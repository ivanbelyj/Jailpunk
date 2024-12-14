using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISectorPlanningStrategy
{
    void PlanSector(GeneratedSectorInfo sector, GenerationContext context);
}
