//namespace LineWars.Model
//{
//    public class EdgeProjection : IReadOnlyEdgeProjection
//    {
//        public Edge Edge { get; private set; }
//        public LineType Type { get; set; }
//        public int CurrentHp { get; set; }

//        public EdgeProjection(Edge edge)
//        {
//            Edge = edge;
//            Type = Edge.LineType;
//            CurrentHp = edge.CurrentHp;
//        }

//        public EdgeProjection(IReadOnlyEdgeProjection edgeProjection)
//        {
//            Edge = edgeProjection.Edge;
//            Type = edgeProjection.Type;
//            CurrentHp = edgeProjection.CurrentHp;
//        }
//    }

//    public interface IReadOnlyEdgeProjection
//    {
//        public Edge Edge { get; }
//        public LineType Type { get; }
//        public int CurrentHp { get; }
//    }
//}
