using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZoneFillingStrategy
{
    void Apply(
        GeneratedZone generatedZone,
        GenerationContext context);
}
