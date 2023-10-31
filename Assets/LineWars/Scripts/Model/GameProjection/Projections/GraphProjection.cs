using DataStructures;
using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class GraphProjection
        : GraphForGame<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>, IReadOnlyGraphProjection
    {
        public IndexList<NodeProjection> NodesIndexList { get; private set; }
        public IndexList<EdgeProjection> EdgesIndexList { get; private set; }
        public IndexList<UnitProjection> UnitsIndexList { get; private set; }

        public GraphProjection(IEnumerable<NodeProjection> nodes, IEnumerable<EdgeProjection> edges)
            : base(nodes, edges)
        { 
            NodesIndexList = new IndexList<NodeProjection>();
            EdgesIndexList = new IndexList<EdgeProjection>();
            UnitsIndexList = new IndexList<UnitProjection>();

            foreach (var node in nodes)
                AddNode(node);

            foreach(var edge in edges)
                EdgesIndexList.Add(edge.Id, edge);

            foreach(var unit in UnitsIndexList.Values)
                unit.InitializeActions(this);
        }

        private void AddNode(NodeProjection node)
        {
            NodesIndexList.Add(node.Id, node);
            if (node.LeftUnit != null)
                AddUnit(node.LeftUnit);
            if (node.RightUnit != null && node.RightUnit != node.LeftUnit)
                AddUnit(node.RightUnit);
            node.UnitAdded += OnUnitAdded;
        }

        private void AddUnit(UnitProjection unit)
        {
            UnitsIndexList.Add(unit.Id, unit);
            unit.Died += OnUnitDied;
        }

        private void OnUnitDied(UnitProjection unit)
        {
            UnitsIndexList.Remove(unit.Id);
        }

        private void OnUnitAdded(UnitProjection unit)
        {
            if(unit == null) return;
            if(UnitsIndexList.ContainsKey(unit.Id)) return;
            else
            {
                if (unit.HasId)
                {
                    UnitsIndexList.Add(unit.Id, unit);
                }
                else
                {
                    var id = UnitsIndexList.Add(unit);
                    unit.SetId(id);
                }
            }
        }

        public static GraphProjection GetCopy(IReadOnlyGraphProjection projection, 
            IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew)
        {
            var oldNodesToNew = new Dictionary<NodeProjection, NodeProjection>();
            var oldEdgesToNew = new Dictionary<EdgeProjection, EdgeProjection>();

            foreach(var oldNode in projection.Nodes)
            {
                var newNode = new NodeProjection(oldNode);
                if(oldNode.Owner != null)
                {
                    var ownerProjection = oldPlayersToNew[oldNode.Owner];
                    newNode.ConnectTo(ownerProjection);
                    if(oldNode.IsBase)
                    {
                        ownerProjection.Base = newNode;
                    }
                }

                if(oldNode.LeftUnit != null)
                {
                    ConnectUnit(oldNode.LeftUnit, newNode, UnitDirection.Left, oldPlayersToNew);
                    if(oldNode.RightUnit == oldNode.LeftUnit)
                    {
                        newNode.RightUnit = newNode.LeftUnit;
                    }
                }
                if(oldNode.RightUnit != null && oldNode.RightUnit != oldNode.LeftUnit)
                {
                    ConnectUnit(oldNode.RightUnit, newNode, UnitDirection.Right, oldPlayersToNew);
                }

                oldNodesToNew[oldNode] = newNode;
            }

            foreach(var oldEdge in projection.Edges)
            {
                var firstNode = oldNodesToNew[oldEdge.FirstNode];
                var secondNode = oldNodesToNew[oldEdge.SecondNode];
                var newEdge = new EdgeProjection(oldEdge, firstNode, secondNode);
                firstNode.AddEdge(newEdge);
                secondNode.AddEdge(newEdge);

                oldEdgesToNew[oldEdge] = newEdge;    
            }

            return new GraphProjection(oldNodesToNew.Values, oldEdgesToNew.Values);
        }

        private static void ConnectUnit(UnitProjection oldUnit, NodeProjection newNode, UnitDirection unitDirection,
            in IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew)
        {
            var newUnit = new UnitProjection(oldUnit, newNode);
            var owner = oldPlayersToNew[oldUnit.Owner];
            newUnit.ConnectTo(owner);
            //newUnit.Died += owner.RemoveOwned;
            switch(unitDirection)
            {
                case UnitDirection.Any:
                case UnitDirection.Right:
                    newNode.RightUnit = newUnit;
                    break;
                case UnitDirection.Left:
                    newNode.LeftUnit = newUnit;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static GraphProjection GetProjectionFromMono(MonoGraph monoGraph,
            IReadOnlyDictionary<BasePlayer, BasePlayerProjection> players)
        {
            var nodeList = new Dictionary<Node, NodeProjection>();
            var edgeList = new Dictionary<Edge, EdgeProjection>();

            foreach (var oldNode in monoGraph.Nodes)
            {
                var score = 1;
                if(oldNode.TryGetComponent<NodeScore>(out NodeScore nodeScore))
                {
                    score = nodeScore.Score;
                }
                var nodeProjection = new NodeProjection(oldNode, score);
                if (oldNode.Owner != null)
                {
                    var nodeOwnerProjection = players[oldNode.Owner];
                    nodeProjection.ConnectTo(nodeOwnerProjection);
                    if(oldNode.IsBase)
                    {
                        nodeOwnerProjection.Base = nodeProjection;
                    }
                }
                if (!oldNode.LeftIsFree)
                {
                    var leftUnitProjection = InitializeUnitFromMono(oldNode.LeftUnit);
                    leftUnitProjection.Node = nodeProjection;
                    nodeProjection.LeftUnit = leftUnitProjection;
                    if(oldNode.RightUnit == oldNode.LeftUnit)
                    {
                        nodeProjection.RightUnit = leftUnitProjection;
                    }
                }
                if (!oldNode.RightIsFree && oldNode.RightUnit != oldNode.LeftUnit)
                {
                    var rightUnitProjection = InitializeUnitFromMono(oldNode.RightUnit);
                    rightUnitProjection.Node = nodeProjection;
                    nodeProjection.RightUnit = rightUnitProjection;
                }

                nodeList[oldNode] = nodeProjection;
            }

            foreach (var edge in monoGraph.Edges)
            {
                var firstNode = nodeList[edge.FirstNode];
                var secondNode = nodeList[edge.SecondNode];
                var edgeProjection = new EdgeProjection(edge, firstNode, secondNode);
                firstNode.AddEdge(edgeProjection);
                secondNode.AddEdge(edgeProjection);

                edgeList[edge] = edgeProjection;
            }

            return new GraphProjection(nodeList.Values, edgeList.Values);

            UnitProjection InitializeUnitFromMono(Unit unit)
            {
                var unitProjection = new UnitProjection(unit);
                var ownerProjection = players[unit.Owner];

                unitProjection.ConnectTo(ownerProjection);
                return unitProjection;
            }
        }
    }

    public interface IReadOnlyGraphProjection
    {
        public IReadOnlyList<NodeProjection> Nodes { get; }
        public IReadOnlyList<EdgeProjection> Edges { get; }
    }
}

