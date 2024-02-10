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
        [SerializeField] private GameObject maskRendererObj;
        [SerializeField, Min(0)] private int maxVisibilityMapSide = 128;
        [SerializeField, Range(1, 255)] private int colorStep = 1000;
        [SerializeField] private bool generateOnStart = false;

        private void Start()
        {
            if(generateOnStart)
                GenerateMap(MonoGraph.Instance);
        }

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

            var visibilityMapTextureSize = CustomMath.TransferVector2ByMaxSide(
                new Vector2Int(fogSprite.texture.width, fogSprite.texture.height),
                maxVisibilityMapSide);

            var visibilityMap = FogUtilities.GenerateVisibilityMapTexture(
                visibilityMapTextureSize,
                monoGraph.Nodes.Select(x => x.Position - minPointOfMap)
                    .Select(x => x.GetPixelCoord(mapSize, visibilityMapTextureSize)),
                colorStep);


            var maskRenderer = maskRendererObj.GetComponent<IMaskRenderer>();
            Debug.Log(maskRenderer.GetType().Name);
            maskRenderer.MapRenderer = map;

            maskRenderer.VisibilityMap = visibilityMap;
            maskRenderer.FogTexture = fogSprite.texture;
            maskRenderer.TargetRenderer.size = mapSize;
            maskRenderer.TargetRenderer.transform.position = center;

            foreach (var node in monoGraph.Nodes)
                maskRenderer.AddRenderNode(node.RenderNodeV3);

            maskRenderer.Initialise();
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