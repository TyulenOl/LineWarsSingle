using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GraphEditor;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoGraph : MonoBehaviour, IGraphForGame<Node, Edge, Unit>
    {
        public static MonoGraph Instance { get; private set; }

        private GraphForGame<Node, Edge, Unit> modelGraph;
        [field: SerializeField] public GameObject NodesParent { get; set; }
        [field: SerializeField] public GameObject EdgesParent { get; set; }

        [SerializeField] private bool autoInitialize = true;
        
        public IReadOnlyList<Node> Nodes => modelGraph.Nodes;
        public IReadOnlyList<Edge> Edges => modelGraph.Edges;
        
        public INodeIndexer<Node, Edge> IdToNode => modelGraph.IdToNode;
        public IEdgeIndexer<Node, Edge> IdToEdge => modelGraph.IdToEdge;

        private bool initialized;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError("Более одного графа!");
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            if (autoInitialize && !initialized)
                AutoInitializeInitialize();
        }

        private void AutoInitializeInitialize()
        {
            initialized = true;
            var allNodes = FindObjectsOfType<Node>();
            var allEdges = FindObjectsOfType<Edge>();
            
            modelGraph = new GraphForGame<Node, Edge, Unit>(allNodes, allEdges);
        }

        public void Initialize()
        {
            if (initialized)
                return;
            initialized = true;
            modelGraph = new GraphForGame<Node, Edge, Unit>();
        }
        
        public void AddNode(Node node)
        {
            modelGraph.AddNode(node);
        }

        public bool RemoveNode(Node node)
        {
            return modelGraph.RemoveNode(node);
        }

        public bool RemoveNode(int nodeId)
        {
            return modelGraph.RemoveNode(nodeId);
        }

        public bool ContainsNode(Node node)
        {
            return modelGraph.ContainsNode(node);
        }

        public bool ContainsNode(int nodeId)
        {
            return modelGraph.ContainsNode(nodeId);
        }

        public void AddEdge(Edge edge)
        {
            modelGraph.AddEdge(edge);
        }

        public bool RemoveEdge(Edge edge)
        {
            return modelGraph.RemoveEdge(edge);
        }

        public bool RemoveEdge(int edgeId)
        {
            return modelGraph.RemoveEdge(edgeId);
        }

        public bool ContainsEdge(Edge edge)
        {
            return modelGraph.ContainsEdge(edge);
        }

        public bool ContainsEdge(int edgeId)
        {
            return modelGraph.ContainsEdge(edgeId);
        }

        public Dictionary<Node, bool> GetVisibilityInfo(BasePlayer player)
            => modelGraph.GetVisibilityInfo(player.MyNodes);

        public IEnumerable<Node> GetVisibilityNodes(IEnumerable<Node> ownedNodes)
            => modelGraph.GetVisibilityNodes(ownedNodes);

        public List<Node> FindShortestPath(
            [NotNull] Node start,
            [NotNull] Node end,
            Func<Node, Node, bool> condition = null)
        {
            return modelGraph.FindShortestPath(start, end, condition);
        }

        public IEnumerable<Node> GetNodesInRange(
            Node startNode,
            uint range,
            Func<Node, Node, bool> condition = null)
        {
            return modelGraph.GetNodesInRange(startNode, range, condition);
        }

        public IEnumerable<Node> MultiStartsLimitedBfs(
            IReadOnlyDictionary<Node, int> startNodes,
            IReadOnlyDictionary<Node, int> interferingNodes)
        {
            return modelGraph.MultiStartsLimitedBfs(startNodes, interferingNodes);
        }

        public IEnumerable<(Node, uint)> FindDistanceToNodes(Node rootNode)
        {
            return modelGraph.FindDistanceToNodes(rootNode);
        }

        public HashSet<(Node, Node)> FindMostRemoteNodes()
        {
            return modelGraph.FindMostRemoteNodes();
        }
    }
}