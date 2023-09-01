using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

public class MaskRendererV3 : MonoBehaviour
{
    public MaskRendererV3 Instance { get; private set; }

    [Header("Settings")] 
    [SerializeField] private bool autoInitialize;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate = 60;
    
    [Header("Map")] 
    [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0.001f, 0.5f)] private float sensitivity = 0.05f;
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
    private static readonly int sensitivityId = Shader.PropertyToID("_Sensitivity");

    private static readonly int blurRadiusId = Shader.PropertyToID("_BlurRadius");

    private static readonly int sourceId = Shader.PropertyToID("_Source");
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

        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var nodeBuffer = nodeBuffers[i];
            needUpdate = needUpdate || Math.Abs(nodeBuffer.Visibility - node.Visibility) > 0.001f;
            nodeBuffer.Visibility = node.Visibility;
            nodeBuffers[i] = nodeBuffer;
        }

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
        
        UpdateHash();

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

        ApplyChanges();
    }


    private void InitializeMaskShader()
    {
        maskShader.SetTexture(0, visibilityMapId, visibilityMap);
        maskShader.SetTexture(0, visibilityMaskId, visibilityMask);
        maskShader.SetInt(nodesCountId, nodes.Count);

        InitializeBuffer();
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
        maskShader.SetBuffer(0, nodesBufferId, buffer);
    }

    private void InitializeBlur()
    {
        blurShader.SetTexture(blurHorID, sourceId, tempSource);
        blurShader.SetTexture(blurHorID, horBlurOutputId, horBlurOutput);

        blurShader.SetTexture(blurVerID, horBlurOutputId, horBlurOutput);
        blurShader.SetTexture(blurVerID, verBlurOutputId, verBlurOutput);
    }

    private bool CheckValid()
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

    private Color GetNodeColor(RenderNodeV3 renderNode)
    {
        var position = renderNode.transform.position;

        var x = Mathf.CeilToInt((position.x - startPosition.position.x) /
            (endPosition.position.x - startPosition.position.x) * visibilityMap.width);
        var y = Mathf.CeilToInt((position.y - startPosition.position.y) /
            (endPosition.position.y - startPosition.position.y) * visibilityMap.height);
        return visibilityMap.GetPixel(x, y);
    }

    private void ApplyChanges()
    {
        buffer.SetData(nodeBuffers);
        maskShader.SetFloat(sensitivityId, sensitivity);

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
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear
        )
        {
            enableRandomWrite = true
        };
        result.Create();
        return result;
    }

    private bool HashIsUpdated()
    {
        bool isUpdated = blurRadiusHash != blurRadius || Math.Abs(sensitivityHash - sensitivity) > 0.0001f;
        return isUpdated;
    }

    private void UpdateHash()
    {
        blurRadiusHash = blurRadius;
        sensitivityHash = sensitivity;
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