using DataStructures;
using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class GraphProjection :
        GraphForGame<NodeProjection, EdgeProjection, UnitProjection>,
        IReadOnlyGraphProjection
    {
        public GameProjection Game { get; set; }  
        public IndexList<NodeProjection> NodesIndexList { get; set; }
        public IndexList<EdgeProjection> EdgesIndexList { get; set; }
        public IndexList<UnitProjection> UnitsIndexList { get; set; }

        public GraphProjection(IEnumerable<NodeProjection> nodes, IEnumerable<EdgeProjection> edges, GameProjection game)
            : base(nodes, edges)
        {
            NodesIndexList = new IndexList<NodeProjection>();
            EdgesIndexList = new IndexList<EdgeProjection>();
            UnitsIndexList = new IndexList<UnitProjection>();
            Game = game;
            foreach (var node in nodes)
                AddNode(node);

            foreach (var edge in edges)
                AddEdge(edge);

            foreach (var unit in UnitsIndexList.Values)
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
            node.Game = Game;
        }

        private void AddEdge(EdgeProjection edge)
        {
            EdgesIndexList.Add(edge.Id, edge);
        }

        private void AddUnit(UnitProjection unit)
        {
            UnitsIndexList.Add(unit.Id, unit);
            unit.Died += OnUnitDied;
            unit.Game = Game;
        }

        private void OnUnitDied(UnitProjection unit)
        {
            UnitsIndexList.Remove(unit.Id);
        }

        private void OnUnitAdded(UnitProjection unit)
        {
            if (unit == null) return;
            if (UnitsIndexList.ContainsKey(unit.Id)) return;
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
            IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew, GameProjection gameProjection)
        {
            var oldNodesToNew = new Dictionary<NodeProjection, NodeProjection>();
            var oldEdgesToNew = new Dictionary<EdgeProjection, EdgeProjection>();

            foreach (var oldNode in projection.Nodes)
            {
                var newNode = NodeProjectionCreator.FromProjection(oldNode);
                if (oldNode.Owner != null)
                {
                    var ownerProjection = oldPlayersToNew[oldNode.Owner];
                    newNode.ConnectTo(ownerProjection);
                    if (oldNode.IsBase)
                    {
                        ownerProjection.Base = newNode;
                    }
                }

                if (oldNode.LeftUnit != null)
                {
                    ConnectUnit(oldNode.LeftUnit, newNode, UnitDirection.Left, oldPlayersToNew);
                    if (oldNode.RightUnit == oldNode.LeftUnit)
                    {
                        newNode.RightUnit = newNode.LeftUnit;
                    }
                }

                if (oldNode.RightUnit != null && oldNode.RightUnit != oldNode.LeftUnit)
                {
                    ConnectUnit(oldNode.RightUnit, newNode, UnitDirection.Right, oldPlayersToNew);
                }

                oldNodesToNew[oldNode] = newNode;
            }

            foreach (var oldEdge in projection.Edges)
            {
                var firstNode = oldNodesToNew[oldEdge.FirstNode];
                var secondNode = oldNodesToNew[oldEdge.SecondNode];
                var newEdge =  EdgeProjectionCreator.FromProjection(oldEdge, firstNode, secondNode);
                firstNode.AddEdge(newEdge);
                secondNode.AddEdge(newEdge);

                oldEdgesToNew[oldEdge] = newEdge;
            }

            return new GraphProjection(oldNodesToNew.Values, oldEdgesToNew.Values, gameProjection);
        }

        private static void ConnectUnit(UnitProjection oldUnit, NodeProjection newNode, UnitDirection unitDirection,
            in IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew)
        {
            var newUnit = UnitProjectionCreator.FromProjection(oldUnit, newNode);
            var owner = oldPlayersToNew[oldUnit.Owner];
            newUnit.ConnectTo(owner);
            //newUnit.Died += owner.RemoveOwned;
            switch (unitDirection)
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
            IReadOnlyDictionary<BasePlayer, BasePlayerProjection> players, GameProjection gameProjection)
        {
            var nodeList = new Dictionary<Node, NodeProjection>();
            var edgeList = new Dictionary<Edge, EdgeProjection>();
            
            //создает ноды
            foreach (var oldNode in monoGraph.Nodes)
            {
                var nodeProjection = NodeProjectionCreator.FromMono(oldNode);
                //присоединяет овнера
                if (oldNode.Owner != null)
                {
                    var nodeOwnerProjection = players[oldNode.Owner];
                    nodeProjection.ConnectTo(nodeOwnerProjection);
                    if (oldNode.IsBase)
                    {
                        nodeOwnerProjection.Base = nodeProjection;
                    }
                }
                //добавляет ноды
                if (!oldNode.LeftIsFree)
                {
                    var leftUnitProjection = InitializeUnitFromMono(oldNode.LeftUnit);
                    leftUnitProjection.Node = nodeProjection;
                    nodeProjection.LeftUnit = leftUnitProjection;
                    if (oldNode.RightUnit == oldNode.LeftUnit)
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
            //добавляет еджы
            foreach (var edge in monoGraph.Edges)
            {
                var firstNode = nodeList[edge.FirstNode];
                var secondNode = nodeList[edge.SecondNode];
                var edgeProjection = EdgeProjectionCreator.FromMono(edge, firstNode, secondNode);
                firstNode.AddEdge(edgeProjection);
                secondNode.AddEdge(edgeProjection);

                edgeList[edge] = edgeProjection;
            }

            return new GraphProjection(nodeList.Values, edgeList.Values, gameProjection);

            UnitProjection InitializeUnitFromMono(Unit unit)
            {
                var unitProjection = UnitProjectionCreator.FromMono(unit);
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