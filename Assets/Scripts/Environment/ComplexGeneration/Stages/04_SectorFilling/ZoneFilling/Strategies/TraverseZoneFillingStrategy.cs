using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TraverseZoneFillingStrategy : ZoneFillingStrategyBase
{
    protected readonly Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory;

    public TraverseZoneFillingStrategy(Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory)
    {
        this.traverseRectFilterFactory = traverseRectFilterFactory;
    }

    public override void Apply(GeneratedZone generatedZone, GenerationContext context)
    {
        var area = context.ComplexData.Scheme.Areas
            .FirstOrDefault(x => x.Id == generatedZone.SchemeAreaId);
            
        if (area == null) {
            // Todo: is it okay?
            return;
        }

        var traverseFilter = traverseRectFilterFactory?.Invoke(generatedZone);
        
        ApplyToScheme(context.ComplexData.Scheme, area, traverseFilter);
    }

    public virtual void ApplyToScheme(
        ComplexScheme scheme,
        SchemeArea area,
        TraverseRectFilter traverseFilter)
    {
        TraverseRectUtils.TraverseRect(area.Rect, (data) => {
            var tile = scheme.GetTileByPos(data.x, data.y);

            if (traverseFilter != null) {
                if (!data.IsFilterSatisfied(traverseFilter)) {
                    return;
                }
            }
            
            ApplyPosition(area, tile, data);
        });
    }

    public abstract void ApplyPosition(
        SchemeArea area,
        SchemePosition tile,
        TraverseRectData traverseData);
}
