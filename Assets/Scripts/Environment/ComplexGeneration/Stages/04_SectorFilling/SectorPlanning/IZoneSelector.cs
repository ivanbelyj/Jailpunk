using UnityEngine;

public interface IZoneSelector
{
    ZoneFillingInfo GenerateZoneFillingInfo(
        GeneratedSectorInfo generatedSectorInfo,
        GenerationRequest generationRequest);
}
