//namespace LineWars.Model
//{
//    public class NodeProjection : IReadOnlyNodeProjection
//    {
//        public Node Node { get; private set; }
//        public ComponentUnit LeftUnit { get; set; }
//        public ComponentUnit RightUnit { get; set; }
//        public bool IsDirty { get; set; }


//        public NodeProjection(Node node)
//        {
//            Node = node;
//            LeftUnit = Node.LeftUnit;
//            RightUnit = Node.RightUnit;
//            IsDirty = node.IsDirty;
//        }

//        public NodeProjection(IReadOnlyNodeProjection projection)
//        {
//            Node = projection.Node;
//            LeftUnit = projection.LeftUnit;
//            RightUnit = projection.RightUnit;
//            IsDirty = projection.IsDirty;
//        }
//    }

//    public interface IReadOnlyNodeProjection
//    {
//        public Node Node { get; }
//        public ComponentUnit LeftUnit { get; }
//        public ComponentUnit RightUnit { get; }
//        public bool IsDirty { get; }
//        public bool LeftIsFree => LeftUnit == null;
//        public bool RightIsFree => RightUnit == null;
//        public bool AnyIsFree => LeftIsFree || RightIsFree;
//        public bool AllIsFree => LeftIsFree && RightIsFree;
//    }
//}
