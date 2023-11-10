using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public static class GraphProjectionCreator
    {
        public static GraphProjection FromMono(
            MonoGraph monoGraph,
            IReadOnlyDictionary<BasePlayer, BasePlayerProjection> players,
            GameProjection gameProjection)
        {
            var nodeList = new Dictionary<Node, NodeProjection>();
            var edgeList = new Dictionary<Edge, EdgeProjection>();


            foreach (var oldNode in monoGraph.Nodes)
            {
                var nodeProjection = NodeProjectionCreator.FromMono(oldNode);

                if (oldNode.Owner != null)
                {
                    var nodeOwnerProjection = players[oldNode.Owner];
                    nodeProjection.ConnectTo(nodeOwnerProjection);
                    if (oldNode.IsBase)
                    {
                        nodeOwnerProjection.Base = nodeProjection;
                    }
                }

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

        public static GraphProjection FromProjection(
            IReadOnlyGraphProjection projection,
            IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew,
            GameProjection gameProjection)
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
                var newEdge = EdgeProjectionCreator.FromProjection(oldEdge, firstNode, secondNode);
                firstNode.AddEdge(newEdge);
                secondNode.AddEdge(newEdge);

                oldEdgesToNew[oldEdge] = newEdge;
            }

            return new GraphProjection(oldNodesToNew.Values, oldEdgesToNew.Values, gameProjection);
        }

        private static void ConnectUnit(
            UnitProjection oldUnit,
            NodeProjection newNode, 
            UnitDirection unitDirection,
            in IReadOnlyDictionary<BasePlayerProjection, BasePlayerProjection> oldPlayersToNew)
        {
            var newUnit = UnitProjectionCreator.FromProjection(oldUnit, newNode);
            var owner = oldPlayersToNew[oldUnit.Owner];
            newUnit.ConnectTo(owner);

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
    }
}