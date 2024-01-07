using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LineWars.Controllers
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private List<MapSpriteFogSprite> randomTextures;
        [SerializeField] private float mapPaddingsInUnits = 10;
        [SerializeField] private SpriteRenderer map;
        [SerializeField] private MaskRendererV3AutoManagement maskRenderer;
        [SerializeField, Min(0)] private int maxVisibilityMapSide = 128;

        public void GenerateMap(MonoGraph monoGraph)
        {
            var randomPair = GetRandomMapSpriteFogSprite();
            var mainSprite = randomPair.MapSprite;
            var fogSprite = randomPair.FogSprite;
            var minPointOfGraph = GetMinPoint(monoGraph) - new Vector2(mapPaddingsInUnits, mapPaddingsInUnits);
            var maxPointOfGraph = GetMaxPoint(monoGraph) + new Vector2(mapPaddingsInUnits, mapPaddingsInUnits);

            var graphSize = maxPointOfGraph - minPointOfGraph;
            var maxSize = Mathf.Max(graphSize.x, graphSize.y);
            var mapSize = CustomMath.TransferProportions(
                new Vector2(maxSize, maxSize),
                new Vector2(mainSprite.texture.width, mainSprite.texture.height)
            );

            var center = minPointOfGraph + graphSize / 2;
            var minPointOfMap = center - mapSize / 2;
            var maxPointOfMap = center + mapSize / 2;
            
            map.sprite = mainSprite;
            map.size = mapSize;
            map.transform.position = center;

            var visibilityMap = GenerateVisibilityMapTexture(
                mapSize,
                fogSprite.texture,
                monoGraph.Nodes.Select(x => x.Position - minPointOfMap));
            
            
            
            maskRenderer.StartPosition.transform.position = minPointOfMap;
            maskRenderer.EndPosition.transform.position = maxPointOfMap;

            maskRenderer.VisibilityMap = visibilityMap;
            maskRenderer.FogTexture = fogSprite.texture;
            maskRenderer.TargetRenderer.size = mapSize;
            maskRenderer.TargetRenderer.transform.position = center;

            foreach (var node in monoGraph.Nodes)
                maskRenderer.AddRenderNode(node.RenderNodeV3);
            
            maskRenderer.Initialise();
        }

        private Texture2D GenerateVisibilityMapTexture(
            Vector2 mapSizeInUnits,
            Texture fogTexture,
            IEnumerable<Vector2> nodesPositionsInWorldSpace)
        {
            var textureSize = CustomMath.TransferVector2ByMaxSide(
                new Vector2Int(fogTexture.width, fogTexture.height),
                maxVisibilityMapSide);
            var totalPixelsCount = textureSize.x * textureSize.y;
            var visibilityMap = new Texture2D(textureSize.x, textureSize.y);
            
            var nodesPositionsInPixels = nodesPositionsInWorldSpace
                .Select(x => x.GetPixelCoord(mapSizeInUnits, textureSize))
                .Where(x => x.CheckPixelCoord(textureSize))
                .Distinct()
                .ToArray();
            
            var colors = nodesPositionsInPixels
                .Select((x, i) =>
                {
                    i += 1;
                    var r = i % 255;
                    var g = i / 255 % 255;
                    var b = i / 65025 % 255;
                    return new Color(
                        Convert.ToSingle(r) / 255f,
                        Convert.ToSingle(g) / 255f,
                        Convert.ToSingle(b) / 255f);
                }).ToArray();
            
            var nextQueues = new Queue<Vector2Int>[nodesPositionsInPixels.Length];
            for (var i = 0; i < nextQueues.Length; i++)
                nextQueues[i] = new Queue<Vector2Int>();
            
            var nextsVisited = new HashSet<Vector2Int>[nodesPositionsInPixels.Length];
            for (var i = 0; i < nextsVisited.Length; i++)
                nextsVisited[i] = new HashSet<Vector2Int>();
            
            var visitedPixels = new HashSet<Vector2Int>(totalPixelsCount);
            var currentQueues = new Queue<Vector2Int>[nodesPositionsInPixels.Length];
            for (var i = 0; i < currentQueues.Length; i++)
            {
                var node = nodesPositionsInPixels[i];
                currentQueues[i] = new Queue<Vector2Int>();
                currentQueues[i].Enqueue(node);
                visitedPixels.Add(node);
                visibilityMap.SetPixel(node.x, node.y, colors[i]);
            }
            
            var currentRadius = 1;
            while (currentQueues.All(x => x.Count != 0))
            {
                for (var i = 0; i < nodesPositionsInPixels.Length; i++)
                {
                    var center = nodesPositionsInPixels[i];
                    var currentQueue = currentQueues[i];
                    var nextQueue = nextQueues[i];
                    var color = colors[i];
                    var visitedNext = nextsVisited[i];

                    while (currentQueue.Count != 0)
                    {
                        var current = currentQueue.Dequeue();
                        
                        foreach (var neighbour in GetNeighboursPixels(current))
                        {
                            if (!neighbour.CheckPixelCoord(textureSize))
                                continue;
                            if (visitedPixels.Contains(neighbour))
                                continue;
                            if (visitedNext.Contains(neighbour))
                                continue;
                            
                            var inCircle = CustomMath.PointInCircle(neighbour, center, currentRadius);
                            if (inCircle)
                            {
                                currentQueue.Enqueue(neighbour);
                                visitedPixels.Add(neighbour);
                                visibilityMap.SetPixel(neighbour.x, neighbour.y, color);
                            }
                            else
                            {
                                visitedNext.Add(neighbour);
                                nextQueue.Enqueue(neighbour);
                            }
                        }
                    }

                    currentQueues[i] = nextQueue;
                    nextQueues[i] = currentQueue;
                    visitedNext.Clear();
                }

                currentRadius++;
            }
            
            visibilityMap.Apply();
            return visibilityMap;
        }

        private IEnumerable<Vector2Int> GetNeighboursPixels(Vector2Int vector2)
        {
            yield return new Vector2Int(vector2.x, vector2.y + 1);
            yield return new Vector2Int(vector2.x + 1, vector2.y);
            yield return new Vector2Int(vector2.x, vector2.y - 1);
            yield return new Vector2Int(vector2.x - 1, vector2.y);
        }

        private MapSpriteFogSprite GetRandomMapSpriteFogSprite()
        {
            return randomTextures[Random.Range(0, randomTextures.Count)];
        }


        private Vector2 GetMinPoint(MonoGraph monoGraph)
        {
            var minX = monoGraph.Nodes.Min(x => x.transform.position.x);
            var minY = monoGraph.Nodes.Min(x => x.transform.position.y);

            return new Vector2(minX, minY);
        }

        private Vector2 GetMaxPoint(MonoGraph monoGraph)
        {
            var maxX = monoGraph.Nodes.Max(x => x.transform.position.x);
            var maxY = monoGraph.Nodes.Max(x => x.transform.position.y);

            return new Vector2(maxX, maxY);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var texture in randomTextures)
                texture.OnValidate();
        }
#endif
    }
}