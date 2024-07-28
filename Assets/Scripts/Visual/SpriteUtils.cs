using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SpriteUtils
{
    public const int PixelsPerUnit = 64;
    private static Tile emptyTile;

    /// <summary>
    /// Common empty tile scriptable object
    /// </summary>
    public static Tile EmptyTile => emptyTile;

    static SpriteUtils() {
        emptyTile = ScriptableObject.CreateInstance<Tile>();
        emptyTile.sprite = GenerateWhiteSprite();
    }

    public static Texture2D GenerateWhiteTexture(int width, int height)
    {
        Texture2D whiteTexture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                whiteTexture.SetPixel(i, j, Color.white);
            }
        }
        whiteTexture.Apply();

        return whiteTexture;
    }

    private static Sprite GenerateWhiteSprite() {
        var whiteTexture = GenerateWhiteTexture(PixelsPerUnit, PixelsPerUnit);
        var sprite = Sprite.Create(
            whiteTexture,
            new Rect(0, 0, whiteTexture.width, whiteTexture.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit: PixelsPerUnit);
        return sprite;
    }
}
