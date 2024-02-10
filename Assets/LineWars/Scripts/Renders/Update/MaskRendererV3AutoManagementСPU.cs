using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using UnityEngine;

public class MaskRendererV3AutoManagementСPU : Singleton<MaskRendererV3AutoManagementСPU>,
    IMaskRenderer
{
    [Header("Settings")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private SpriteRenderer mapRenderer;
    [SerializeField, Min(0)] private int numberFramesSkippedBeforeUpdate;

    [Header("Map")] 
    [SerializeField] private Texture2D visibilityMap;
    [SerializeField] private Texture2D fogTexture;
    [SerializeField] private Texture2D fogEffect;
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField, Range(0, 1)] private float cutoff = 0.3f;
    [SerializeField] private Color edgeColor = Color.black;
    [SerializeField] private Color fogColor = Color.white;
    [SerializeField] [Range(0, 10)] private int blurRadius = 2;

    [Header("Shaders")]
    private Material fogShader;

    [Space]
    [SerializeField] private bool autoInitialize;
    
    private List<RenderNodeV3> startNodes = new();
    private Dictionary<RenderNodeV3, List<Vector2Int>> nodePixelsPairs;
    private Dictionary<RenderNodeV3, NodeCash> nodeCash;
    private List<RenderNodeV3> nodes;
    
    private HashSet<RenderNodeV3> updatesNodes;
    
    private Texture2D visibilityMask;
    
    private float[,] blurInput;
    private float[,] blurOutput;
    private float[,] tempBlur;
    
    private static readonly int fogColorId = Shader.PropertyToID("_FogColor");
    private static readonly int edgeColorId = Shader.PropertyToID("_EdgeColor");
    private static readonly int fogTextureId = Shader.PropertyToID("_FogTex");
    private static readonly int fogEffectId = Shader.PropertyToID("_FogEff");
    private static readonly int visibilityMaskId = Shader.PropertyToID("_VisibilityMaskV3");
    private static readonly int noiseTextureId = Shader.PropertyToID("_Noise");
    private static readonly int cutoffParameterId = Shader.PropertyToID("_Cutoff");

    private bool initialized;

    private bool applyStarted;
    private bool needUpdate;

    private const string FogShaderName = "Custom/FogV3Updated";

    public SpriteRenderer TargetRenderer
    {
        get => targetRenderer;
        set => targetRenderer = value;
    }

    public SpriteRenderer MapRenderer
    {
        get => mapRenderer;
        set => mapRenderer = value;
    }

    public Texture2D VisibilityMap
    {
        get => visibilityMap;
        set => visibilityMap = value;
    }
    public Texture2D FogTexture
    {
        get => fogTexture;
        set => fogTexture = value;
    }
    
    private struct NodeData
    {
        public RenderNodeV3 Node;
        public Color Color;
        public Vector2Int CoordInTexture;
    }
    
    private struct NodeCash
    {
        public float Visibility;
    }
    
    private void Start()
    {
        if (autoInitialize)
        {
            startNodes = FindObjectsOfType<RenderNodeV3>().ToList();
            Initialise();
        }
    }
    
    public void Initialise()
    {
        if (initialized)
        {
            Debug.LogError($"{nameof(MaskRendererV3AutoManagementСPU)} is Initialized!");
            return;
        }
        if (!CheckValid()) return;
        initialized = true;
        //targetRenderer.size = mapRenderer.size;
        
        visibilityMask = CreateTexture2D();
        
        InitializeBlur();
        InitializeFogShader();
        ProcessNodes();
        
        ApplyChanges();
    }

    private void InitializeBlur()
    {
        blurInput = new float[visibilityMap.width, visibilityMap.height];
        blurOutput = new float[visibilityMap.width, visibilityMap.height];
        tempBlur = new float[visibilityMap.width, visibilityMap.height];
    }
    
    private void ProcessNodes()
    {
        var bounds = mapRenderer.bounds;
        var size = bounds.size;
        var startPosition = bounds.center - size / 2;
        
        var nodesData = GetNodesData(startPosition, size).ToArray();
        var colorNodePairs = nodesData.ToDictionary(x => x.Color, x => x.Node);
        nodePixelsPairs = GetNodePixelsPairs(nodesData, colorNodePairs);
        nodeCash = nodesData.ToDictionary(x => x.Node, x => new NodeCash() {Visibility = x.Node.Visibility});
        nodes = nodesData.Select(x => x.Node).ToList();
        updatesNodes = new HashSet<RenderNodeV3>(nodes);
    }

    private Dictionary<RenderNodeV3, List<Vector2Int>> GetNodePixelsPairs(
        IReadOnlyCollection<NodeData> nodesData,
        IReadOnlyDictionary<Color, RenderNodeV3> colorNodePairs)
    {
        var result = new Dictionary<RenderNodeV3, List<Vector2Int>>(nodesData.Count);
        foreach (var node in nodesData.Select(x => x.Node))
            result.Add(node, new List<Vector2Int>());

        for (var x = 0; x < visibilityMap.width; x++)
        for (var y = 0; y < visibilityMap.height; y++)
        {
            var pixelColor = visibilityMap.GetPixel(x, y);
            if (colorNodePairs.TryGetValue(pixelColor, out var node))
                result[node].Add(new Vector2Int(x, y));
        }

        return result;
    }

    private IEnumerable<NodeData> GetNodesData(Vector3 startPosition, Vector3 size)
    {
        foreach (var node in startNodes)
        {
            var pixelCoord = (node.transform.position - startPosition)
                .To2D()
                .GetPixelCoord(size, visibilityMap.GetTextureSize());

            if (pixelCoord.CheckPixelCoord(visibilityMap.GetTextureSize()))
            {
                yield return new NodeData()
                {
                    Node = node,
                    CoordInTexture = pixelCoord,
                    Color = pixelCoord.GetPixelColor(visibilityMap)
                };
            }
            else
                Debug.LogWarning($"Нода {node.name} не имеет цвета");
        }
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

        fogShader.SetTexture(visibilityMaskId, visibilityMask);
        targetRenderer.sharedMaterial = fogShader;
    }
    
    private void Update()
    {
        if (!initialized) return;
        needUpdate = needUpdate || UpdateHash();
        if (needUpdate && !applyStarted)
        {
            StartCoroutine(ApplyChangesCoroutine());
        }
    }

    private void OnValidate()
    {
        if(Application.isPlaying)
            needUpdate = true;
    }

    private IEnumerator ApplyChangesCoroutine()
    {
        applyStarted = true;
        for (var i = 0; i < numberFramesSkippedBeforeUpdate; i++)
            yield return null;
        UpdateHash();
        ApplyChanges();
        applyStarted = false;
        needUpdate = false;
    }
    
    private void ApplyChanges()
    {
        var updatedPixels = updatesNodes.Select(x => (x, nodePixelsPairs[x]));
        CalculateVisibilityMask(blurInput, updatedPixels);
        CalculateBlur(blurInput, tempBlur, blurOutput, blurRadius);
        SetAlfaChannel(visibilityMask, blurOutput);
        
        updatesNodes.Clear();
    }
    
    private bool UpdateHash()
    {
        var isUpdated = false;

        foreach (var node in nodes)
        {
            if (Math.Abs(node.Visibility - nodeCash[node].Visibility) > 0.0001f)
            {
                isUpdated = true;
                nodeCash[node] = new NodeCash {Visibility = node.Visibility};
                updatesNodes.Add(node);
            }
        }
        
        return isUpdated;
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

        startNodes.Add(node);
    }
    bool CheckValid()
    {
        if (targetRenderer == null)
        {
            Debug.LogError($"{nameof(targetRenderer)} is null!");
            return false;
        }
        
        if (mapRenderer == null)
        {
            Debug.LogError($"{nameof(mapRenderer)} is null!");
            return false;
        }

        if (mapRenderer.sprite == null)
        {
            Debug.LogError($"{nameof(mapRenderer.sprite)} is null!");
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

        if (startNodes == null || startNodes.Count == 0)
        {
            Debug.LogError($"{nameof(startNodes)} is empty!");
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
    private Texture2D CreateTexture2D()
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
    
    private static void CalculateVisibilityMask(
        float[,] data,
        IEnumerable<(RenderNodeV3, List<Vector2Int>)> nodePixelsPairs)
    {
        foreach (var (node, pixels) in nodePixelsPairs)
        foreach (var pixel in pixels)
            data[pixel.x, pixel.y] = node.Visibility;
    }

    private static void CalculateBlur(
        float[,] blurInput,
        float[,] temp,
        float[,] blurOutput,
        int blurRadius)
    {
        CalculateHorBlur(blurInput, temp, blurRadius);
        CalculateVertBlur(temp, blurOutput, blurRadius);
    }

    private static void CalculateHorBlur(float[,] blurInput, float[,] blurOutput, int blurRadius)
    {
        var width = blurInput.GetLength(0);
        var height = blurInput.GetLength(1);
        
        var startScale = Math.Min(1 + blurRadius, width);
        for (var y = 0; y < height; y++)
        {
            var currentScale = startScale;
            var sum = 0f;
            for (var x = 0; x < currentScale; x++)
                sum += blurInput[x, y];
            blurOutput[0, y] = sum / currentScale;
            
            for (var x = 1; x < width; x++)
            {
                var nextIndex = x + blurRadius;
                if (nextIndex < width)
                {
                    sum += blurInput[nextIndex, y];
                    currentScale++;
                }
                
                var prevIndex = x - blurRadius-1;
                if (prevIndex >= 0)
                {
                    sum -= blurInput[prevIndex, y];
                    currentScale--;
                }

                blurOutput[x, y] = sum / currentScale;
            }
        }
    }
    private static void CalculateVertBlur(float[,] blurInput, float[,] blurOutput, int blurRadius)
    {
        var width = blurInput.GetLength(0);
        var height = blurInput.GetLength(1);
        
        var startScale = Math.Min(1 + blurRadius, height);
        for (var x = 0; x < width; x++)
        {
            var currentScale = startScale;
            var sum = 0f;
            for (var y = 0; y < currentScale; y++)
                sum += blurInput[x, y];
            blurOutput[x, 0] = sum / currentScale;
            
            for (var y = 1; y < height; y++)
            {
                var nextIndex = y + blurRadius;
                if (nextIndex < height)
                {
                    sum += blurInput[x, nextIndex];
                    currentScale++;
                }
                
                var prevIndex = y - blurRadius-1;
                if (prevIndex >= 0)
                {
                    sum -= blurInput[x, prevIndex];
                    currentScale--;
                }

                blurOutput[x, y] = sum / currentScale;
            }
        }
    }

    private static void SetAlfaChannel(Texture2D texture, float[,] alpha)
    {
        for (var x = 0; x < texture.width; x++)
        for (var y = 0; y < texture.height; y++)
            texture.SetPixel(x, y, new Color(0,0,0, alpha[x,y]));
        texture.Apply();
    }
}