using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    [SerializeField]
    private Sprite floorTileSprite;

    [SerializeField]
    private Sprite wallTileSprite;

    private Dictionary<uint, Tile> tilesById = new();

    public Tile GetTile(uint tileId) {
        // TODO: temporary hardcoded
        if (!tilesById.TryGetValue(tileId, out var tile)) {
            tile = CreateTile(tileId switch {
                1 => floorTileSprite,
                2 => wallTileSprite,
                _ => throw new NotImplementedException()
            });
            tilesById.Add(tileId, tile);
        }
        return tile;
    }

    private Tile CreateTile(Sprite sprite) {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        return tile;
    }
}
