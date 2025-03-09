using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    private readonly Dictionary<string, Tile> tilesById = new();

    private AssetManager assetManager;
    private AppearanceSpriteResolver appearanceSpriteResolver;

    private void Awake() {
        assetManager = FindAnyObjectByType<AssetManager>();
        appearanceSpriteResolver = FindAnyObjectByType<AppearanceSpriteResolver>();
    }

    public Tile GetTile(SchemeTile schemeTile)
    {
        var tileKey = GetKey(schemeTile);
        if (!tilesById.TryGetValue(tileKey, out var tile)) {
            var mapObjectSchema = GetMapObjectSchema(schemeTile.MapObjectName);
            tile = CreateTile(GetTileSprite(mapObjectSchema));
            tilesById.Add(tileKey, tile);
        }
        return tile;
    }

    private string GetKey(SchemeTile schemeTile) => $"{schemeTile.MapObjectName}";

    private Tile CreateTile(Sprite sprite) {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        return tile;
    }

    private Sprite GetTileSprite(MapObjectSchema mapObjectSchema) {
        // Other appearance sprite data are not supported for now
        return appearanceSpriteResolver.Resolve(new() {
            Name = mapObjectSchema.appearanceName
        });
    }

    private MapObjectSchema GetMapObjectSchema(string mapObjectName)
    {
        return assetManager.MapObjectSchemas.GetAssetById(mapObjectName);
    }
}
