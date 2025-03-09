using UnityEngine;

public abstract class ZoneFillingStrategyBase : IZoneFillingStrategy
{
    public int Id { get; set; }

    public abstract void Apply(GeneratedZone generatedZone, GenerationContext context);
}
