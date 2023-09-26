using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class NodeExtension
    {
        public static bool HasLine(this ModelNode from, ModelNode to)
        {
            return GetLine(from, to) != null;
        }
        
        public static ModelEdge GetLine(this ModelNode from, ModelNode to)
        {
            return from.Edges.Intersect(to.Edges).FirstOrDefault();
        }

        public static List<ModelNode> FindShortestPath(this ModelNode start, ModelNode end)
        {
            return Graph.FindShortestPath(start, end);
        }

        public static IEnumerable<ITarget> GetTargetsWithMe(this ModelNode node)
        {
            yield return node;
            if (node.LeftUnit != null)
                yield return node.LeftUnit;
            if (node.RightUnit != null)
                yield return node.RightUnit;
        }
    }
}