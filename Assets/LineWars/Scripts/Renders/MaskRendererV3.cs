﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

public class MaskRendererV3 : MonoBehaviour
{
    public MaskRendererV3 Instance { get; private set; }

    [Header("Settings")] [SerializeField] private bool autoInitialize;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate = 60;

    [Header("Map")] [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0, 10)] private int blurRadius;

    [Header("Shaders")] [SerializeField] private ComputeShader maskShader;
    [SerializeField] private ComputeShader blurShader;

    [Header("")] [SerializeField] private List<RenderNodeV3> nodes;
    private List<RenderNodeV3> availableNodes;


    private RenderTexture visibilityMask;

    private RenderTexture tempSource;
    private RenderTexture verBlurOutput;
    private RenderTexture horBlurOutput;
    private RenderTexture shaderInput;

    private List<NodesBuffer> nodeBuffers;
    private ComputeBuffer buffer;

    private static readonly int nodesCountId = Shader.PropertyToID("_NodesCount");
    private static readonly int nodesBufferId = Shader.PropertyToID("_NodesBuffer");
    private static readonly int visibilityMapId = Shader.PropertyToID("_VisibilityMap");
    private static readonly int visibilityMaskId = Shader.PropertyToID("_VisibilityMaskV3");

    private static readonly int blurRadiusId = Shader.PropertyToID("_BlurRadius");

    private static readonly int sourceId = Shader.PropertyToID("_Source");
    private static readonly int texSizeId = Shader.PropertyToID("_TexSize");
    private static readonly int verBlurOutputId = Shader.PropertyToID("_VerBlurOutput");
    private static readonly int horBlurOutputId = Shader.PropertyToID("_HorBlurOutput");

    private int blurHorID;
    private int blurVerID;

    private float sensitivityHash;
    private int blurRadiusHash;


    private bool initialized;

    private bool applyStarted;
    private bool needUpdate;


    private struct NodesBuffer
    {
        public Color NodeColor;
        public float Visibility;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Too many {nameof(MaskRendererV3)}");
        }
    }

    public void Start()
    {
        nodes = FindObjectsOfType<RenderNodeV3>().ToList();
        if (autoInitialize)
            Initialise();
    }

    private void Update()
    {
        if (!initialized) return;

        needUpdate = needUpdate || HashIsUpdated();
        if (needUpdate && !applyStarted)
        {
            StartCoroutine(ApplyChangesCoroutine());
        }
    }

    private void OnDestroy()
    {
        buffer?.Dispose();
        visibilityMask?.Release();
        horBlurOutput?.Release();
        verBlurOutput?.Release();
        shaderInput?.Release();
    }

    public void Initialise()
    {
        if (initialized)
        {
            Debug.LogError($"{nameof(MaskRendererV3)} is Initialized!");
            return;
        }

        if (!CheckValid()) return;

        initialized = true;

        blurHorID = blurShader.FindKernel("HorzBlurCs");
        blurVerID = blurShader.FindKernel("VertBlurCs");

        tempSource = CreateTexture();
        visibilityMask = CreateTexture();
        horBlurOutput = CreateTexture();
        verBlurOutput = CreateTexture();
        shaderInput = CreateTexture();

        InitializeMaskShader();
        InitializeBlur();

        Shader.SetGlobalTexture(visibilityMaskId, shaderInput);

        UpdateHash();
        ApplyChanges();

        bool CheckValid()
        {
            if (visibilityMap == null)
            {
                Debug.LogError($"{nameof(visibilityMap)} is null!");
                return false;
            }

            if (nodes == null || nodes.Count == 0)
            {
                Debug.LogError($"{nameof(nodes)} is enmpy!");
                return false;
            }

            if (blurShader == null)
            {
                Debug.LogError($"{nameof(blurShader)} is null!");
                return false;
            }

            if (maskShader == null)
            {
                Debug.LogError($"{nameof(maskShader)} is null!");
                return false;
            }

            if (!visibilityMap.isReadable)
            {
                Debug.LogError("Карта видимости не доступна для чтения. Пожалуйста исправте это в настройках импорта");
                return false;
            }

            return true;
        }
    }


    private void InitializeMaskShader()
    {
        maskShader.SetTexture(0, visibilityMapId, visibilityMap);
        maskShader.SetTexture(0, visibilityMaskId, visibilityMask);
        maskShader.SetInt(nodesCountId, nodes.Count);

        InitializeBuffer();
        maskShader.SetBuffer(0, nodesBufferId, buffer);
    }

    private void InitializeBuffer()
    {
        var texSizeInWorldCoord = startPosition.position
            .To2D()
            .GetSize(endPosition.position.To2D());
        
        nodeBuffers = new List<NodesBuffer>(nodes.Count);
        availableNodes = new List<RenderNodeV3>();
        foreach (var node in nodes)
        {
            var pixelCoord = node.transform.position
                    .To2D()
                    .GetPixelCoord(texSizeInWorldCoord, visibilityMap.GetTextureSize());
            if (pixelCoord.CheckPixelCoord(visibilityMap.GetTextureSize()))
            {
                var nodeBuffer = new NodesBuffer()
                {
                    NodeColor = pixelCoord.GetPixelColor(visibilityMap),
                    Visibility = node.Visibility
                };
                nodeBuffers.Add(nodeBuffer);
                availableNodes.Add(node);
            }
        }

        buffer = new ComputeBuffer(nodeBuffers.Count * 5, sizeof(float));

        buffer.SetData(nodeBuffers);
    }

    private void InitializeBlur()
    {
        blurShader.SetTexture(blurHorID, sourceId, tempSource);
        blurShader.SetTexture(blurHorID, horBlurOutputId, horBlurOutput);

        blurShader.SetTexture(blurVerID, horBlurOutputId, horBlurOutput);
        blurShader.SetTexture(blurVerID, verBlurOutputId, verBlurOutput);

        blurShader.SetFloats(texSizeId, visibilityMap.width, visibilityMap.height);
    }

    private void ApplyChanges()
    {
        buffer.SetData(nodeBuffers);

        var x = Mathf.CeilToInt(visibilityMap.width / 8.0f);
        var y = Mathf.CeilToInt(visibilityMap.height / 8.0f);
        maskShader.Dispatch(0, x, y, 1);


        Graphics.Blit(visibilityMask, tempSource);
        blurShader.SetInt(blurRadiusId, blurRadius);
        blurShader.Dispatch(blurHorID, x, visibilityMap.height, 1);
        blurShader.Dispatch(blurVerID, visibilityMap.width, y, 1);
        Graphics.Blit(verBlurOutput, shaderInput);
    }


    private RenderTexture CreateTexture()
    {
        var result = new RenderTexture
        (
            visibilityMap.width,
            visibilityMap.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Default
        )
        {
            enableRandomWrite = true
        };
        result.Create();
        return result;
    }

    private bool HashIsUpdated()
    {
        bool isUpdated = blurRadiusHash != blurRadius;

        for (var i = 0; i < nodes.Count; i++)
        {
            if (isUpdated)
                break;

            var node = nodes[i];
            var nodeBuffer = nodeBuffers[i];
            isUpdated = Math.Abs(nodeBuffer.Visibility - node.Visibility) > 0.001f;
        }

        return isUpdated;
    }

    private void UpdateHash()
    {
        blurRadiusHash = blurRadius;

        for (var i = 0; i < availableNodes.Count; i++)
        {
            var nodeBuffer = nodeBuffers[i];
            nodeBuffer.Visibility = availableNodes[i].Visibility;
            nodeBuffers[i] = nodeBuffer;
        }
    }

    public void AddRenderNode(RenderNodeV3 node)
    {
        if (node == null)
        {
            Debug.LogError("Node cant be null!");
            return;
        }

        if (initialized)
        {
            Debug.LogError("Cant add node after initialize!");
            return;
        }

        nodes.Add(node);
    }

    private IEnumerator ApplyChangesCoroutine()
    {
        applyStarted = true;
        for (int i = 0; i < numberFramesSkippedBeforeUpdate; i++)
            yield return null;
        UpdateHash();
        ApplyChanges();
        applyStarted = false;
        needUpdate = false;
    }
}

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