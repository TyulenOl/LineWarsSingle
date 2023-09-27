//using System.Collections.Generic;
//using System.Linq;

//namespace LineWars.Model
//{
//    public class GraphProjection : IReadOnlyGraphProjection
//    {
//        public Dictionary<Node, NodeProjection> Nodes { get; set; }
//        public Dictionary<Edge, EdgeProjection> Edges { get; set; }
//        public Dictionary<ComponentUnit, UnitProjection> Units { get; set; }

//        public IReadOnlyDictionary<Node, IReadOnlyNodeProjection> AllNodes 
//            => (IReadOnlyDictionary<Node, IReadOnlyNodeProjection>) Nodes;

//        public IReadOnlyDictionary<Edge, IReadOnlyEdgeProjection> AllEdges 
//            => (IReadOnlyDictionary<Edge, IReadOnlyEdgeProjection>) Edges;

//        public IReadOnlyDictionary<ComponentUnit, IReadOnlyUnitProjection> AllUnits 
//            => (IReadOnlyDictionary<ComponentUnit, IReadOnlyUnitProjection>) Units;

//        public GraphProjection(Graph graph)
//        {
//            Nodes = graph.Nodes.ToDictionary(node => node, node => new NodeProjection(node));
//            Edges = graph.Edges.ToDictionary(edge => edge, edge => new EdgeProjection(edge));

//            Units = new Dictionary<ComponentUnit, UnitProjection>();
//            foreach(var node in graph.Nodes)
//            {
//                if(node.LeftUnit != null)
//                    Units[node.LeftUnit] = new UnitProjection(node.LeftUnit);
//                if(node.RightIsFree) 
//                    Units[node.RightUnit] = new UnitProjection(node.RightUnit);
//            }
//        }

//        public GraphProjection(IReadOnlyGraphProjection projection) 
//        {
//            Nodes = new Dictionary<Node, NodeProjection>();
//            foreach(var nodeInfo in projection.AllNodes)
//                Nodes[nodeInfo.Key] = new NodeProjection(nodeInfo.Value);

//            Edges = new Dictionary<Edge, EdgeProjection>();
//            foreach(var edgeInfo in projection.AllEdges)
//                Edges[edgeInfo.Key] = new EdgeProjection(edgeInfo.Value);

//            Units = new Dictionary<ComponentUnit, UnitProjection>();
//            foreach(var unitInfo  in projection.AllUnits)
//                Units[unitInfo.Key] = new UnitProjection(unitInfo.Value);

//        }
//    }

//    public interface IReadOnlyGraphProjection
//    {
//        IReadOnlyDictionary<Node, IReadOnlyNodeProjection> AllNodes { get; }
//        IReadOnlyDictionary<Edge, IReadOnlyEdgeProjection> AllEdges { get; }
//        IReadOnlyDictionary<ComponentUnit, IReadOnlyUnitProjection> AllUnits { get; }

//    }
//}
