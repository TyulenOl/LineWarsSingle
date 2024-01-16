using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

public class MaskRendererV3AutoManagement : MonoBehaviour
{
    public MaskRendererV3AutoManagement Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate;

    [Header("Map")] 
    [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    [SerializeField] private Texture2D fogTexture;
    [SerializeField] private Texture2D fogEffect;
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField, Range(0, 1)] private float cutoff = 0.3f;
    [SerializeField] private Color edgeColor = Color.black;
    [SerializeField] private Color fogColor = Color.white;
    [SerializeField] [Range(0, 10)] private int blurRadius = 2;

    [Header("Shaders")]
    [SerializeField] private ComputeShader maskShader;
    [SerializeField] private ComputeShader blurShader;
    private Material fogShader;

    [Space]
    [SerializeField] private bool autoInitialize;
    [SerializeField] private List<RenderNodeV3> nodes;
    private List<RenderNodeV3> availableNodes;


    private RenderTexture visibilityMask;

    private RenderTexture tempSource;
    private RenderTexture verBlurOutput;
    private RenderTexture horBlurOutput;
    private RenderTexture shaderInput;

    private List<NodesBuffer> nodeBuffers;
    private ComputeBuffer buffer;
    
    
    private static readonly int fogTextureId = Shader.PropertyToID("_FogTex");
    private static readonly int fogEffectId = Shader.PropertyToID("_FogEff");
    private static readonly int noiseTextureId = Shader.PropertyToID("_Noise");
    private static readonly int cutoffParameterId = Shader.PropertyToID("_Cutoff");
    private static readonly int edgeColorId = Shader.PropertyToID("_EdgeColor");
    private static readonly int fogColorId = Shader.PropertyToID("_FogColor");

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

    private const string FogShaderName = "Custom/FogV3Updated";

    public SpriteRenderer TargetRenderer => targetRenderer;
    public Texture2D VisibilityMap
    {
        get => visibilityMap;
        set => visibilityMap = value;
    }

    public Transform StartPosition
    {
        get => startPosition;
        set => startPosition = value;
    }

    public Transform EndPosition
    {
        get => endPosition;
        set => endPosition = value;
    }

    public Texture2D FogTexture
    {
        get => fogTexture;
        set => fogTexture = value;
    }


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
        if (autoInitialize)
        {
            nodes = FindObjectsOfType<RenderNodeV3>().ToList();
            Initialise();
        }
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
        InitializeFogShader();

        UpdateHash();
        ApplyChanges();
    }

    private void InitializeFogShader()
    {
        fogShader = new Material(GetFogShader());
        fogShader.SetTexture(fogTextureId, fogTexture);
        fogShader.SetTexture(fogEffectId, fogEffect);
        fogShader.SetTexture(noiseTextureId, noiseTexture);
        fogShader.SetFloat(cutoffParameterId, cutoff);
        fogShader.SetColor(fogColorId, fogColor);
        fogShader.SetColor(edgeColorId, edgeColor);

        fogShader.SetTexture(visibilityMaskId, shaderInput);
        targetRenderer.sharedMaterial = fogShader;
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
            var pixelCoord = (node.transform.position - startPosition.position)
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
            else
            {
                Debug.LogWarning($"Нода {node.name} не имеет цвета");
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

    bool CheckValid()
    {
        if (targetRenderer == null)
        {
            Debug.LogError($"{nameof(targetRenderer)} is null!");
            return false;
        }

        if (fogTexture == null)
        {
            Debug.LogError($"{nameof(fogTexture)} is null!");
            return false;
        }

        if (fogEffect == null)
        {
            Debug.LogError($"{nameof(fogEffect)} is null!");
            return false;
        }

        if (noiseTexture == null)
        {
            Debug.LogError($"{nameof(noiseTexture)} is null!");
            return false;
        }

        if (visibilityMap == null)
        {
            Debug.LogError($"{nameof(visibilityMap)} is null!");
            return false;
        }

        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogError($"{nameof(nodes)} is empty!");
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

        if (GetFogShader() == null)
        {
            Debug.LogError($"Не удалось найти шейдер тумана войны [{FogShaderName}]");
            return false;
        }

        return true;
    }

    private Shader GetFogShader()
    {
        return Shader.Find(FogShaderName);
    }
}