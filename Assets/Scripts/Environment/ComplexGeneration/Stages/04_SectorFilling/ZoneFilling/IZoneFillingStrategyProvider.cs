using System.Collections.Generic;
using UnityEngine;

public interface IZoneFillingStrategyProvider
{
    List<IZoneFillingStrategy> GetZoneFillingStrategies(GeneratedZone generatedZone);
}
