using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FogUtilities
{
    public static Texture2D GenerateVisibilityMapTexture(
        Vector2Int visibilityTextureSize,
        IEnumerable<Vector2Int> nodesPositionsInWorldSpace,
        int colorStep,
        TextureFormat textureFormat = TextureFormat.RGBA32,
        bool mipMap = false)
    {
        var totalPixelsCount = visibilityTextureSize.x * visibilityTextureSize.y;
        var visibilityMap = new Texture2D(visibilityTextureSize.x, visibilityTextureSize.y, textureFormat, mipMap);

        var centers = nodesPositionsInWorldSpace
            .Where(x => x.CheckPixelCoord(visibilityTextureSize))
            .Distinct()
            .ToArray();

        var colors = GetColors(centers.Length, colorStep);

        var nextQueues = new Queue<Vector2Int>[centers.Length];
        for (var i = 0; i < nextQueues.Length; i++)
            nextQueues[i] = new Queue<Vector2Int>();

        var coloredPixels = new HashSet<Vector2Int>(totalPixelsCount);
        var currentQueues = new Queue<Vector2Int>[centers.Length];
        for (var i = 0; i < currentQueues.Length; i++)
        {
            var node = centers[i];
            currentQueues[i] = new Queue<Vector2Int>();
            currentQueues[i].Enqueue(node);
            coloredPixels.Add(node);
            visibilityMap.SetPixel(node.x, node.y, colors[i]);
        }

        var claimedPixels = new HashSet<Vector2Int>(); // занятые

        var currentRadius = 1;
        while (currentQueues.Any(x => x.Count != 0))
        {
            for (var i = 0; i < centers.Length; i++)
            {
                var center = centers[i];
                var currentQueue = currentQueues[i];
                var nextQueue = nextQueues[i];
                var color = colors[i];

                while (currentQueue.Count != 0)
                {
                    var current = currentQueue.Dequeue();

                    foreach (var neighbour in GetNeighboursPixels(current))
                    {
                        if (!neighbour.CheckPixelCoord(visibilityTextureSize))
                            continue;
                        if (coloredPixels.Contains(neighbour))
                            continue;
                        if (claimedPixels.Contains(neighbour))
                            continue;

                        var inCircle = CustomMath.PointInCircle(
                            neighbour,
                            center,
                            currentRadius);

                        if (inCircle)
                        {
                            currentQueue.Enqueue(neighbour);
                            coloredPixels.Add(neighbour);
                            visibilityMap.SetPixel(neighbour.x, neighbour.y, color);
                        }
                        else
                        {
                            claimedPixels.Add(neighbour);
                            nextQueue.Enqueue(neighbour);
                        }
                    }
                }

                currentQueues[i] = nextQueue;
                nextQueues[i] = currentQueue;
            }

            claimedPixels.Clear();
            currentRadius++;
        }

        var uncoloredPixels = FogExtensions.GetAllPixels(visibilityTextureSize)
            .Where(x => !coloredPixels.Contains(x))
            .ToArray();

        foreach (var uncoloredPixel in uncoloredPixels)
        {
            var minDist = float.MaxValue;
            var colorToPixel = new Color();
            for (var i = 0; i < centers.Length; i++)
            {
                var dist = (centers[i] - uncoloredPixel).magnitude;
                if (dist < minDist)
                {
                    colorToPixel = colors[i];
                    minDist = dist;
                }
            }

            visibilityMap.SetPixel(uncoloredPixel.x, uncoloredPixel.y, colorToPixel);
        }

        visibilityMap.Apply();
        return visibilityMap;
    }


    public static Color[] GetColors(int colorsCount, int colorStep)
    {
        var colors = new Color[colorsCount];
        for (var i = 0; i < colorsCount; i++)
        {
            var colorValue = colorStep * i + 1;
            var r = colorValue % 255;
            var g = colorValue / 255 % 255;
            var b = colorValue / 65025 % 255;
            colors[i] = new Color(
                Convert.ToSingle(r) / 255f,
                Convert.ToSingle(g) / 255f,
                Convert.ToSingle(b) / 255f);
        }

        return colors;
    }

    public static IEnumerable<Vector2Int> GetNeighboursPixels(Vector2Int vector2)
    {
        yield return new Vector2Int(vector2.x, vector2.y + 1);
        yield return new Vector2Int(vector2.x + 1, vector2.y);
        yield return new Vector2Int(vector2.x, vector2.y - 1);
        yield return new Vector2Int(vector2.x - 1, vector2.y);
    }
}