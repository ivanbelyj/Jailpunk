using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TraverseZoneFillingStrategyOptions
{

}

public abstract class TraverseZoneFillingStrategy : ZoneFillingStrategyBase
{
    #region Context Types
    public class TraverseZoneFillingStrategyContext : ZoneFillingStrategyContext {
        public TraverseRectFilter TraverseFilter { get; set; }

        public TraverseZoneFillingStrategyContext(
            ZoneFillingStrategyContext context,
            TraverseRectFilter traverseRectFilter)
            : base(context.GenerationContext, context.GeneratedZone, context.Area)
        {
            TraverseFilter = traverseRectFilter;
        }
    }

    public class TraverseZoneFillingStrategyPositionContext : TraverseZoneFillingStrategyContext {
        public SchemePosition Position { get; set; }
        public TraverseRectData TraverseData { get; set; }

        public TraverseZoneFillingStrategyPositionContext(
            TraverseZoneFillingStrategyContext context,
            SchemePosition position,
            TraverseRectData traverseRectData)
            : base(context, context.TraverseFilter)
        {
            Position = position;
            TraverseData = traverseRectData;
        }
    }
    #endregion

    protected readonly Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory;
    
    public TraverseZoneFillingStrategy(Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory)
    {
        this.traverseRectFilterFactory = traverseRectFilterFactory;
    }

    public sealed override void Apply(ZoneFillingStrategyContext context)
    {
        var traverseRectFilter = traverseRectFilterFactory?.Invoke(context.GeneratedZone);
        var newContext = new TraverseZoneFillingStrategyContext(context, traverseRectFilter);
        Apply(newContext);
    }

    public virtual void Apply(TraverseZoneFillingStrategyContext context)
    {
        TraverseRectUtils.TraverseRect(context.Area.Rect, (data) => {
            var tile = context.GenerationContext.ComplexData.Scheme.GetTileByPos(data.x, data.y);

            if (context.TraverseFilter != null) {
                if (!data.IsFilterSatisfied(context.TraverseFilter)) {
                    return;
                }
            }
            
            ApplyPosition(
                new TraverseZoneFillingStrategyPositionContext(context, tile, data));
        });
    }

    public abstract void ApplyPosition(TraverseZoneFillingStrategyPositionContext context);
}
