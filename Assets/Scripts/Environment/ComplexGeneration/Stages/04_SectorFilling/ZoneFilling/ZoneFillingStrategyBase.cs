using System.Linq;
using UnityEngine;

public abstract class ZoneFillingStrategyBase : IZoneFillingStrategy
{
    public class ZoneFillingStrategyContext {
        public GenerationContext GenerationContext { get; set; }
        public GeneratedZone GeneratedZone { get; set; }
        public SchemeArea Area { get; set; }
        public ZoneFillingStrategyContext(
            GenerationContext generationContext,
            GeneratedZone generatedZone,
            SchemeArea schemeArea)
        {
            GenerationContext = generationContext;
            GeneratedZone = generatedZone;
            Area = schemeArea;
        }
    }

    public void Apply(GeneratedZone generatedZone, GenerationContext context)
    {
        var area = context.ComplexData.Scheme.Areas.FirstOrDefault(
            x => x.Id == generatedZone.SchemeAreaId);
            
        if (area == null) {
            // Todo: is it okay?
            return;
        }

        Apply(new(context, generatedZone, area));
    }

    public abstract void Apply(ZoneFillingStrategyContext context);
}
