using System;
using UnityEngine;

public static class FogExtensions
{
    public static Vector2 To2D(this Vector3 vector3)
    {
        return vector3;
    }

    public static Vector2 GetSize(this Vector2 start, Vector2 end)
    {
        if (end.x < start.x || end.y < start.y)
            throw new ArgumentException();
        return end - start;
    }

    public static Vector2Int GetTextureSize(this Texture2D texture2D)
    {
        return new Vector2Int(texture2D.width, texture2D.height);
    }

    public static Vector2Int GetPixelCoord(
        this Vector2 pointPositionInWorldCoord,
        Vector2 texSizeInWorldCoord,
        Vector2Int textureSizeInPixels)
    {
        var x = Mathf.CeilToInt(pointPositionInWorldCoord.x / texSizeInWorldCoord.x * textureSizeInPixels.x);
        var y = Mathf.CeilToInt(pointPositionInWorldCoord.y / texSizeInWorldCoord.y * textureSizeInPixels.y);
        return new Vector2Int(x, y);
    }

    public static bool CheckPixelCoord(
        this Vector2Int pixelCoord,
        Vector2Int textureSizeInPixels)
    {
        return pixelCoord.x > 0
               && pixelCoord.x < textureSizeInPixels.x
               && pixelCoord.y > 0
               && pixelCoord.y < textureSizeInPixels.y;
    }

    public static Color GetPixelColor(this Vector2Int pixelCoord, Texture2D texture2D)
    {
        if (!pixelCoord.CheckPixelCoord(texture2D.GetTextureSize()))
            throw new ArgumentOutOfRangeException();
        return texture2D.GetPixel(pixelCoord.x, pixelCoord.y);
    }
}