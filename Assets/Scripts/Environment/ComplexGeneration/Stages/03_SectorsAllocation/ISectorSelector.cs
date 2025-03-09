using UnityEngine;

public interface ISectorSelector
{
    SectorPlanningInfo GenerateSectorPlanningInfo(
        GeneratedSectorInfo sectorInfo,
        GenerationRequest generationRequest
    );
}
