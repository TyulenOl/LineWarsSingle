using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utilities.Runtime;

public class AutoVisibilityMapGenerator: MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(1)] private int maxVisibilityMapSide = 128;
    [SerializeField, Min(1)] private int colorStep = 1000;
    
    [SerializeField] private string dirPath = "VisibilityMaps";
    [SerializeField] private string fileName = "VisibilityMap";
    
    [Header("References")]
    [SerializeField] private SpriteRenderer mapRenderer;
    [SerializeField] private List<Transform> nodes;

    private string AbsoluteDirPath => $"{Application.dataPath}/{dirPath}";
    private string AbsoluteFilePath => $"{AbsoluteDirPath}/{fileName}{Guid.NewGuid()}.png";


    public Texture2D GenerateVisibilityMap()
    {
        if (mapRenderer == null || mapRenderer.sprite == null)
            return null;
        
        var bounds = mapRenderer.bounds;
        var mapSize = bounds.size;
        var startPosition = bounds.center - mapSize / 2;
        
        var mapTexture = mapRenderer.sprite.texture;
        var textureSize = mapTexture.GetTextureSize();
        var visibilityMapSize = CustomMath.TransferVector2ByMaxSide(
            textureSize,
            maxVisibilityMapSide); 
        
        return FogUtilities.GenerateVisibilityMapTexture(
            visibilityMapSize,
            nodes.Select(x => (x.position - startPosition).To2D())
                 .Select(x => x.GetPixelCoord(mapSize, visibilityMapSize)),
            colorStep
        );
    }
#if UNITY_EDITOR
    [EditorButton]
    public void GenerateVisibilityMapAndSave()
    {
        if (!Directory.Exists(AbsoluteDirPath))
            Directory.CreateDirectory(AbsoluteDirPath);
        
        var tex = GenerateVisibilityMap();
        var bytes = tex.EncodeToPNG();
        var path = AbsoluteFilePath;
        File.WriteAllBytes(path, bytes);
        UnityEditor.AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }
#endif
}
