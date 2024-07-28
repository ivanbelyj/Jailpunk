using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SpriteUtils
{
    public const int PixelsPerUnit = 64;

    public static Texture2D GenerateTexture(int width, int height, Color color)
    {
        Texture2D whiteTexture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                whiteTexture.SetPixel(i, j, color);
            }
        }
        whiteTexture.Apply();

        return whiteTexture;
    }

    public static Sprite GenerateSprite(Color color) {
        var whiteTexture = GenerateTexture(PixelsPerUnit, PixelsPerUnit, color);
        var sprite = Sprite.Create(
            whiteTexture,
            new Rect(0, 0, whiteTexture.width, whiteTexture.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit: PixelsPerUnit);
        return sprite;
    }
}
