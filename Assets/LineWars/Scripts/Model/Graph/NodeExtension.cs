using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class NodeExtension
    {
        public static bool HasLine(this Node from, Node to)
        {
            return GetLine(from, to) != null;
        }
        
        public static Edge GetLine(this Node from, Node to)
        {
            return from.Edges.Intersect(to.Edges).FirstOrDefault();
        }

        public static List<Node> FindShortestPath(this Node start, Node end)
        {
            return Graph.FindShortestPath(start, end);
        }
    }
}