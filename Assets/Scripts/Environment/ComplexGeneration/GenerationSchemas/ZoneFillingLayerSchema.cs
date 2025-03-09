using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneFillingLayerSchema {
    public int order;

    public ZoneFillingType fillingType;

    public SchemePositionType targetPositionType;

    [Tooltip(
        "Allows to use existing layer (or add it otherwise). Leave empty to add new layer anyway")]
    public string targetLayerName;
    public string mapObjectSchemaAddress;
    public TraverseRectFilter traverseRectFilter;
}
