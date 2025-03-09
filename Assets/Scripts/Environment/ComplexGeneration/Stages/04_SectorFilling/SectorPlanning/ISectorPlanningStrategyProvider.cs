using UnityEngine;

public interface ISectorPlanningStrategyProvider
{
    public ISectorPlanningStrategy GetSectorPlanningStrategy(GeneratedSectorInfo sector);
}
