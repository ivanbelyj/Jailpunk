using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(VisibilityProcessor))]
public class DarknessRenderer : MonoBehaviour
{
    private Tilemap tilemap;
    private VisibilityProcessor visibilityProcessor;

    [SerializeField]
    private Color darknessColor = Color.black;
    private Tile tileDarkness;

    private const string DarknessTilemapTag = "DarknessOverlay";

    private void Awake()
    {
        tilemap = GameObject
            .FindWithTag(DarknessTilemapTag)
            .GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError($"Please, create empty tilemap with tag "
                + $"'{DarknessTilemapTag}' on scene to use {nameof(DarknessRenderer)}");
            return;
        }

        visibilityProcessor = GetComponent<VisibilityProcessor>();

        tileDarkness = CreateEmptyTile(darknessColor);
    }

    private static Tile CreateEmptyTile(Color color) {
        var emptyTile = ScriptableObject.CreateInstance<Tile>();
        emptyTile.sprite = SpriteUtils.GenerateSprite(color);
        emptyTile.color = Color.white;
        emptyTile.flags = TileFlags.None;
        return emptyTile;
    }

    private void Update() {
        visibilityProcessor.UpdateVisibility();

        visibilityProcessor.VisibilityMap.TraverseVisibilityMap((row, col, x, y) => {
            Vector3Int pos = new(x, y);

            EnsureTileExists(pos);

            float targetDarkness = 1f - visibilityProcessor.VisibilityMap[row, col];
            UpdateTileAlphaSmoothly(pos, targetDarkness);
        });
    }

    private void EnsureTileExists(Vector3Int pos) {
        if (!tilemap.HasTile(pos))
        {
            tilemap.SetTile(pos, tileDarkness);
        }
    }

    /// <param name="darkness">In [0f, 1f]</param>
    private void SetDarknessForTile(Vector3Int pos, float darkness) {
        // tilemap.SetTileFlags(pos, TileFlags.None);
        
        var color = new Color(1f, 1f, 1f, darkness);
        tilemap.SetColor(pos, color);
    }
    
    private void UpdateTileAlphaSmoothly(Vector3Int pos, float targetAlpha) {
        float currentAlpha = tilemap.GetColor(pos).a;
        
        float t = Time.deltaTime * 7f;

        // Very strange fix for darker colors (case 1 -> 0), it just works
        if (targetAlpha < currentAlpha) {
            t *= (darknessColor.r + darknessColor.g + darknessColor.b) / 3f;
            t *= 2f / currentAlpha;
        }
        float newAlpha = currentAlpha + (targetAlpha - currentAlpha) * t;

        SetDarknessForTile(pos, newAlpha);
    }
}
