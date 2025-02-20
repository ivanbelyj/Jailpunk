using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TraverseZoneFillingStrategy : IZoneFillingStrategy
{
    public void Apply(GeneratedZone generatedZone, GenerationContext context)
    {
        var area = context.ComplexData.Scheme.Areas
            .FirstOrDefault(x => x.Id == generatedZone.SchemeAreaId);
            
        if (area == null) {
            // Todo: is it okay?
            return;
        }
        
        ApplyToScheme(context.ComplexData.Scheme, area);
    }

    public virtual void ApplyToScheme(ComplexScheme scheme, SchemeArea area) {
        TraverseRectUtils.TraverseRect(area.Rect, (data) => {
            var tile = scheme.GetTileByPos(data.x, data.y);
            
            ApplyPosition(area, tile, data);
        });
    }

    public abstract void ApplyPosition(
        SchemeArea area,
        SchemePosition tile,
        TraverseRectData traverseData);
}
