using System;
using UnityEngine;

[Serializable]
public class LayerTargetedZoneFillingStrategyOptions : TraverseZoneFillingStrategyOptions
{
    public SchemePositionType targetPositionType;

    [Tooltip(
        "Allows to use existing layer (or add it otherwise). Leave empty to add new layer anyway")]
    public string targetLayerName;
}

public abstract class LayerTargetedZoneFillingStrategy : TraverseZoneFillingStrategy
{
    private readonly LayerTargetedZoneFillingStrategyOptions options;

    public LayerTargetedZoneFillingStrategy(
        Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory,
        LayerTargetedZoneFillingStrategyOptions options) : base(traverseRectFilterFactory)
    {
        this.options = options;
    }

    public abstract void FillPosition(
        TraverseZoneFillingStrategyPositionContext context,
        SchemePosition position,
        SchemeTile schemeTile);

    public sealed override void ApplyPosition(TraverseZoneFillingStrategyPositionContext context)
    {
        var targetLayer = GetTargetLayer(context.Position);
        if (targetLayer != null) 
        {
            FillPosition(context, context.Position, targetLayer);
        }
    }

    private SchemeTile GetTargetLayer(SchemePosition position)
    {
        var targetLayerName = options.targetLayerName;
        var targetPositionType = options.targetPositionType;

        if (position.Type != targetPositionType) {
            return null;
        }
        var layer = string.IsNullOrWhiteSpace(targetLayerName)
            ? position.GetLayerByName(targetLayerName)
            : null;
        if (layer == null) {
            layer = new SchemeTile();
            if (!string.IsNullOrWhiteSpace(targetLayerName)) {
                layer.LayerName = targetLayerName;
            }
            position.Layers.Add(layer);
        }
        return layer;
    }
}
