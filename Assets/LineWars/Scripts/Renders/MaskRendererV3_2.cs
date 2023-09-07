using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

public class MaskRendererV3_2 : MonoBehaviour
{
#if UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    protected const int GPU_MEMORY_BLOCK_SIZE_BLUR = 484;
#elif UNITY_ANDROID
    protected const int GPU_MEMORY_BLOCK_SIZE_BLUR = 64;
#else
    protected const int GPU_MEMORY_BLOCK_SIZE_BLUR = 1024;
#endif
    
    public MaskRendererV3_2 Instance { get; private set; }

    [Header("Settings")] 
    [SerializeField] private bool autoInitialize;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate = 60;
    
    [Header("Map")] 
    [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0, 10)] private int blurRadius;

    [Header("Shaders")]
    [SerializeField] private ComputeShader maskShader;
    [SerializeField] private ComputeShader blurShader;

    [Header("")] 
    [SerializeField] private List<RenderNodeV3> nodes;


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

    private static readonly int blurRadiusId = Shader.PropertyToID("blurRadius");

    private static readonly int sourceId = Shader.PropertyToID("_Source");
    private static readonly int verBlurOutputId = Shader.PropertyToID("_VerBlurOutput");
    private static readonly int horBlurOutputId = Shader.PropertyToID("_HorBlurOutput");

    private int blurHorID;
    private int blurVerID;
    
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
        
        visibilityMask = CreateTexture();

        tempSource = CreateTexture();
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
        buffer = new ComputeBuffer(nodes.Count * 5, sizeof(float));
        nodeBuffers = new List<NodesBuffer>(nodes.Count);
        foreach (var node in nodes)
        {
            var nodeBuffer = new NodesBuffer()
            {
                NodeColor = GetNodeColor(node),
                Visibility = node.Visibility
            };
            nodeBuffers.Add(nodeBuffer);
        }

        buffer.SetData(nodeBuffers);
    }
    
    private void InitializeBlur()
    {
        blurShader.SetTexture(blurHorID, sourceId, tempSource);
        blurShader.SetTexture(blurHorID, horBlurOutputId, horBlurOutput);

        blurShader.SetTexture(blurVerID, horBlurOutputId, horBlurOutput);
        blurShader.SetTexture(blurVerID, verBlurOutputId, verBlurOutput);
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
        buffer.SetData(nodeBuffers);

        var x = Mathf.CeilToInt(visibilityMap.width / 8.0f);
        var y = Mathf.CeilToInt(visibilityMap.height / 8.0f);
        maskShader.Dispatch(0, x, y, 1);

        int horizontalBlurDisX = Mathf.CeilToInt(((float)visibilityMap.width / (float)GPU_MEMORY_BLOCK_SIZE_BLUR)); // it is here becouse res of window can change
        int horizontalBlurDisY = Mathf.CeilToInt(((float)visibilityMap.height / (float)GPU_MEMORY_BLOCK_SIZE_BLUR));

        Graphics.Blit(visibilityMask, tempSource);
        blurShader.SetInt(blurRadiusId, blurRadius);
        blurShader.Dispatch(blurHorID, horizontalBlurDisX, visibilityMap.height, 1);
        blurShader.Dispatch(blurVerID, visibilityMap.width, horizontalBlurDisY, 1);
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
        
        for (var i = 0; i < nodes.Count; i++)
        {
            var nodeBuffer = nodeBuffers[i];
            nodeBuffer.Visibility = nodes[i].Visibility;
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
    
    private void OnDestroy()
    {
        buffer?.Dispose();
        visibilityMask?.Release();
        horBlurOutput?.Release();
        verBlurOutput?.Release();
        shaderInput?.Release();
    }
}