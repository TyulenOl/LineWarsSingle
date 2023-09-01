using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

public class MaskRendererV3_1 : MonoBehaviour
{
    public MaskRendererV3_1 Instance { get; private set; }

    [Header("Settings")] [SerializeField] private bool autoInitialize;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate = 60;

    [Header("Map")] [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0, 10)] private int blurRadius;

    [SerializeField] private ComputeShader blurShader;

    [Header("")] 
    [SerializeField] private List<RenderNodeV3> nodes;


    private Texture2D visibilityMask;

    private RenderTexture verBlurOutput;
    private RenderTexture horBlurOutput;
    private RenderTexture shaderInput;


    private static readonly int visibilityMaskId = Shader.PropertyToID("_VisibilityMaskV3");

    private static readonly int blurRadiusId = Shader.PropertyToID("_BlurRadius");

    private static readonly int sourceId = Shader.PropertyToID("_Source");
    private static readonly int verBlurOutputId = Shader.PropertyToID("_VerBlurOutput");
    private static readonly int horBlurOutputId = Shader.PropertyToID("_HorBlurOutput");

    private int blurHorID;
    private int blurVerID;
    
    private int blurRadiusHash;


    private bool initialized;

    private bool applyStarted;
    private bool needUpdate;

    private Dictionary<RenderNodeV3, List<(int, int)>> nodeAndPixelsDictionary = new ();
    private Dictionary<RenderNodeV3, float> nodeAndVisibilityHash = new ();

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

        UpdateHash();

        blurHorID = blurShader.FindKernel("HorzBlurCs");
        blurVerID = blurShader.FindKernel("VertBlurCs");

        visibilityMask = CreateTexture();
        horBlurOutput = CreateRenderTexture();
        verBlurOutput = CreateRenderTexture();
        shaderInput = CreateRenderTexture();

        InitializeBlur();
        InitializeNodesDictionary();

        Shader.SetGlobalTexture(visibilityMaskId, shaderInput);

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

            if (!visibilityMap.isReadable)
            {
                Debug.LogError("Карта видимости не доступна для чтения. Пожалуйста исправте это в настройках импорта");
                return false;
            }

            return true;
        }
    }
    
    private void InitializeBlur()
    {
        blurShader.SetTexture(blurHorID, sourceId, visibilityMask);
        blurShader.SetTexture(blurHorID, horBlurOutputId, horBlurOutput);

        blurShader.SetTexture(blurVerID, horBlurOutputId, horBlurOutput);
        blurShader.SetTexture(blurVerID, verBlurOutputId, verBlurOutput);
    }

    private void InitializeNodesDictionary()
    {
        var colorNodeDictionary = new Dictionary<Color, RenderNodeV3>(nodes.Count);
        foreach (var node in nodes)
            colorNodeDictionary[GetNodeColor(node)] = node;
        
        foreach (var node in nodes)
            nodeAndPixelsDictionary[node] = new List<(int, int)>();
        
        for (int x = 0; x < visibilityMap.width; x++)
        {
            for (int y = 0; y < visibilityMap.height; y++)
            {
                var color = visibilityMap.GetPixel(x, y);
                if (colorNodeDictionary.TryGetValue(color, out var node))
                    nodeAndPixelsDictionary[node].Add((x,y));
            }
        }
        Debug.Log("InitializeNodesDictionary success");
    }
    
    private Color GetNodeColor(RenderNodeV3 renderNode)
    {
        var position = renderNode.transform.position;

        var start = startPosition.position;
        var end = endPosition.position;
        
        var x = Mathf.CeilToInt((position.x - start.x) / (end.x - start.x) * visibilityMap.width);
        var y = Mathf.CeilToInt((position.y - start.y) / (end.y - start.y) * visibilityMap.height);
        
        return visibilityMap.GetPixel(x, y);
    }

    private void ApplyChanges()
    {
        RecalculateMask();
        
        var x = Mathf.CeilToInt(visibilityMap.width / 8.0f);
        var y = Mathf.CeilToInt(visibilityMap.height / 8.0f);


        blurShader.SetInt(blurRadiusId, blurRadius);
        blurShader.Dispatch(blurHorID, x, visibilityMap.height, 1);
        blurShader.Dispatch(blurVerID, visibilityMap.width, y, 1);
        Graphics.Blit(verBlurOutput, shaderInput);
    }
    
    private void RecalculateMask()
    {
        foreach (var (node, pixels) in nodeAndPixelsDictionary)
        {
            foreach (var (x, y) in pixels)
            {
                visibilityMask.SetPixel(x, y, new Color(0,0,0,node.Visibility)); 
            }
        }
        visibilityMask.Apply();
    }

    private RenderTexture CreateRenderTexture()
    {
        var result = new RenderTexture
        (
            visibilityMap.width,
            visibilityMap.height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear
        )
        {
            enableRandomWrite = true
        };
        result.Create();
        return result;
    }

    private Texture2D CreateTexture()
    {
        var result = new Texture2D
        (
            visibilityMap.width,
            visibilityMap.height,
            TextureFormat.Alpha8,
            -1,
            false
        );
        return result;
    }

    private bool HashIsUpdated()
    {
        bool isUpdated = blurRadiusHash != blurRadius;
        foreach (var (node, visibility) in nodeAndVisibilityHash)
        {
            if (Math.Abs(node.Visibility - visibility) > 0.000000001f) 
            {
                isUpdated = true;
                break;
            }
        }
        return isUpdated;
    }

    private void UpdateHash()
    {
        blurRadiusHash = blurRadius;
        foreach (var node in nodes)
            nodeAndVisibilityHash[node] = node.Visibility;
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