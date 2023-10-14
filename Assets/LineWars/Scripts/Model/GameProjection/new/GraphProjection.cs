using System.Collections.Generic;

namespace LineWars.Model
{
    public class GraphProjection
        : GraphForGame<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>, IReadOnlyGraphProjection
    {
        public GraphProjection(IEnumerable<NodeProjection> nodes, IEnumerable<EdgeProjection> edges)
            : base(nodes, edges)
        { }

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
                    var newLeftUnit = new UnitProjection(oldNode.LeftUnit, newNode);
                    var leftOwner = oldPlayersToNew[oldNode.LeftUnit.Owner];
                    newLeftUnit.ConnectTo(leftOwner);
                    newNode.LeftUnit = newLeftUnit;
                }
                if(oldNode.RightUnit != null)
                {
                    var newRightUnit = new UnitProjection(oldNode.RightUnit, newNode);
                    var rightOwner = oldPlayersToNew[oldNode.RightUnit.Owner];
                    newRightUnit.ConnectTo(rightOwner);
                    newNode.RightUnit = newRightUnit;
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

        public static GraphProjection GetProjectionFromMono(MonoGraph monoGraph,
            IReadOnlyDictionary<BasePlayer, BasePlayerProjection> players)
        {
            var nodeList = new Dictionary<Node, NodeProjection>();
            var edgeList = new Dictionary<Edge, EdgeProjection>();

            foreach (var node in monoGraph.Nodes)
            {
                var nodeProjection = new NodeProjection(node);
                if (node.Owner != null)
                {
                    var nodeOwnerProjection = players[node.Owner];
                    nodeProjection.ConnectTo(nodeOwnerProjection);
                    if(node.IsBase)
                    {
                        nodeOwnerProjection.Base = nodeProjection;
                    }
                }
                if (!node.LeftIsFree)
                {
                    var leftUnitProjection = InitializeUnitFromMono(node.LeftUnit);
                    leftUnitProjection.Node = nodeProjection;
                    nodeProjection.LeftUnit = leftUnitProjection;
                }
                if (node.RightIsFree)
                {
                    var rightUnitProjection = InitializeUnitFromMono(node.RightUnit);
                    rightUnitProjection.Node = nodeProjection;
                    nodeProjection.RightUnit = rightUnitProjection;
                }

                nodeList[node] = nodeProjection;
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

