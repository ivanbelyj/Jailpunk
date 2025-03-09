using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZoneFillingStrategy
{
    int Id { get; }
    void Apply(
        GeneratedZone generatedZone,
        GenerationContext context);
}
