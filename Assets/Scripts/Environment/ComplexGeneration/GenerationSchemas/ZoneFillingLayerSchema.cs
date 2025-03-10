using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneFillingLayerSchema {
    public int order;

    public ZoneFillingType fillingType;

    public TraverseRectFilter traverseRectFilter;

    public LayerTargetedZoneFillingStrategyOptions layerTargetOptions;
    public string mapObjectSchemaAddress;
}
