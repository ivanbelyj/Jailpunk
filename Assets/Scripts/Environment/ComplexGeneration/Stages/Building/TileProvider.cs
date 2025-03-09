using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    private readonly Dictionary<string, Tile> tilesById = new();
    private Dictionary<string, int> appearanceSpriteDataIndexesByAppearanceSpriteName = new();

    private AssetManager assetManager;
    private AppearanceSpriteResolver appearanceSpriteResolver;

    private void Awake() {
        assetManager = FindAnyObjectByType<AssetManager>();
        appearanceSpriteResolver = FindAnyObjectByType<AppearanceSpriteResolver>();
    }

    public Tile GetTile(SchemeTile schemeTile)
    {
        var mapObjectSchema = GetMapObjectSchema(schemeTile.MapObjectAddress);
        var appearanceSpriteData = GetAppearanceSpriteData(mapObjectSchema);
        var tileKey = GetKey(appearanceSpriteData);
        if (!tilesById.TryGetValue(tileKey, out var tile))
        {
            tile = CreateTile(appearanceSpriteData, mapObjectSchema);
            tilesById.Add(tileKey, tile);
        }
        return tile;
    }

    private string GetKey(AppearanceSpriteData appearanceSpriteData) {
        return AppearanceSpriteResolvingUtils.ToAppearanceDataId(appearanceSpriteData);
    }

    private Tile CreateTile(
        AppearanceSpriteData appearanceSpriteData,
        MapObjectSchema mapObjectSchema)
    {
        var sprite = appearanceSpriteResolver.Resolve(appearanceSpriteData);
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = mapObjectSchema.color;
        return tile;
    }

    private int GetAppearanceSpriteDataIndex(AppearanceSpriteData appearanceSpriteData) {
        var key = appearanceSpriteData.Name;
        if (!appearanceSpriteDataIndexesByAppearanceSpriteName.TryGetValue(
            key,
            out int index))
        {
            index = GetRandomAppearanceSpriteIndex(appearanceSpriteData);
            appearanceSpriteDataIndexesByAppearanceSpriteName.Add(
                key,
                index);
        }
        return index;
    }

    private int GetRandomAppearanceSpriteIndex(AppearanceSpriteData appearanceSpriteData)
        => UnityEngine.Random.Range(0, appearanceSpriteResolver.GetLabelsCount(appearanceSpriteData));

    private AppearanceSpriteData GetAppearanceSpriteData(MapObjectSchema mapObjectSchema) {
        AppearanceSpriteData result;
        switch (mapObjectSchema.mapObjectAppearanceResolvingType) {
            case MapObjectAppearanceResolvingType.RandomIndex:
                result = GetBaseSpriteData(mapObjectSchema);
                result.Index = GetRandomAppearanceSpriteIndex(result);
                break;
            case MapObjectAppearanceResolvingType.RandomIndexForAll:
                result = GetBaseSpriteData(mapObjectSchema);
                result.Index = GetAppearanceSpriteDataIndex(result);
                break;
            case MapObjectAppearanceResolvingType.ByFullAppearanceSpriteData:
                result = mapObjectSchema.appearanceSpriteData;
                break;
            default:
                throw new NotSupportedException();
        };
        return result;
    }

    private AppearanceSpriteData GetBaseSpriteData(MapObjectSchema mapObjectSchema) {
        return new() {
            Name = mapObjectSchema.appearanceSpriteData.Name,
            // Now not used, but maybe it will require additional fix
            // State = mapObjectSchema.appearanceSpriteData.State,
            // Angle = mapObjectSchema.appearanceSpriteData.Angle
        };
    }

    private MapObjectSchema GetMapObjectSchema(string mapObjectAddress)
    {
        return assetManager.MapObjectSchemas.GetAssetById(mapObjectAddress);
    }
}
