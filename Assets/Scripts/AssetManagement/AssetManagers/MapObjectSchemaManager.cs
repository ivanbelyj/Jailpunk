using UnityEngine;

public class MapObjectSchemaManager : BaseAssetManager<MapObjectSchema>
{
    protected override string GetAssetId(MapObjectSchema asset) => asset.mapObjectName;
}
