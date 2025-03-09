using System;
using UnityEngine;

public class SolidZoneFillingStrategy : TraverseZoneFillingStrategy
{
    private readonly SchemePositionType targetPositionType;
    private readonly string targetLayerName;
    private readonly string mapObjectSchemaAddress;

    public SolidZoneFillingStrategy(
        Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory,
        string targetLayerName,
        SchemePositionType targetPositionType,
        string mapObjectSchemaAddress) : base(traverseRectFilterFactory)
    {
        this.targetLayerName = targetLayerName;
        this.targetPositionType = targetPositionType;
        this.mapObjectSchemaAddress = mapObjectSchemaAddress;
    }

    public override void ApplyPosition(
        SchemeArea area,
        SchemePosition position,
        TraverseRectData traverseData)
    {
        var targetLayer = GetTargetLayer(position);
        if (targetLayer != null) 
        {
            FillPosition(position, targetLayer);
        }
    }

    private SchemeTile GetTargetLayer(SchemePosition position)
    {
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

    private void FillPosition(SchemePosition position, SchemeTile targetLayer)
    {
        targetLayer.MapObjectAddress = mapObjectSchemaAddress;
    }
}
