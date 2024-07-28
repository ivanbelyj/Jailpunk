using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(VisibilityProcessor))]
public class DarknessRenderer : MonoBehaviour
{
    private Tilemap tilemap;
    private VisibilityProcessor visibilityProcessor;

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
    }

    private void Update() {
        visibilityProcessor.UpdateVisibility();

        visibilityProcessor.VisibilityMap.TraverseVisibilityMap((row, col, x, y) => {
            Vector3Int pos = new(x, y);

            EnsureTileExists(pos);

            // float visibility = visibilityProcessor.VisibilityMap[row, col];
            // SetDarknessForTile(pos, 1f - visibility);
            float targetVisibility = 1f - visibilityProcessor.VisibilityMap[row, col];
            UpdateTileAlphaSmoothly(pos, targetVisibility);
        });
    }

    private void EnsureTileExists(Vector3Int pos) {
        if (!tilemap.HasTile(pos))
        {
            tilemap.SetTile(pos, SpriteUtils.EmptyTile);
        }
    }

    /// <param name="darkness">In [0f, 1f]</param>
    private void SetDarknessForTile(Vector3Int pos, float darkness) {
        tilemap.SetTileFlags(pos, TileFlags.None);
        tilemap.SetColor(pos, new Color(0f, 0f, 0f, darkness));
    }
    private void UpdateTileAlphaSmoothly(Vector3Int pos, float targetAlpha) {

        float currentAlpha = tilemap.GetColor(pos).a;
        
        // 0 -> 1 - correctly
        // 0 + (1 - 0) * t = допустим 0.1
        // 0.1 + (1 - 0.1) * t

        // 1 -> 0 - wrong
        // 1 + (0 - 1) * t = допустим 0.1
        // 0.9 + (0 - 0.9) * t


        // a + (b - a) * Clamp01(t);
        float newAlpha = Mathf.Lerp(
            currentAlpha,
            targetAlpha,
            Time.deltaTime * 5f);

        SetDarknessForTile(pos, newAlpha);
    }
}
