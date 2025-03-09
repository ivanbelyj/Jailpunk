using System;

public class SectorPlanningStrategyBuilder
{
    private readonly IdGenerator idGenerator;

    public SectorPlanningStrategyBuilder(IdGenerator idGenerator)
    {
        this.idGenerator = idGenerator;
    }

    public ISectorPlanningStrategy BuildStrategyFromSchema(SectorGenerationSchema sectorGenerationSchema) {
        ISectorPlanningStrategy strategy = sectorGenerationSchema.planningType switch {
            SectorPlanningType.RoomsAndCorridors => new SectorRoomPlanningStrategy(idGenerator),
            SectorPlanningType.Glade => new GladePlanningStrategy(idGenerator),
            _ => throw new NotImplementedException()
        };
        return strategy;
    }
}
