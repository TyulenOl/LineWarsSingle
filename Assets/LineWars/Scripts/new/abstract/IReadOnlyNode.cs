using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface IReadOnlyNode : INumbered, IReadOnlyTarget, IReadOnlyOwned
    {
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public IReadOnlyUnit LeftUnit { get; }
        public IReadOnlyUnit RightUnit { get; }

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;

        public bool AllIsFree => LeftIsFree && RightIsFree;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool IsBase { get; }

        public IReadOnlyCollection<IReadOnlyEdge> Edges { get; }

        public IReadOnlyEdge GetLine(IReadOnlyNode node) => Edges.Intersect(node.Edges).FirstOrDefault();

        public IEnumerable<IReadOnlyNode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }

        public bool ContainsEdge(IReadOnlyEdge edge) => Edges.Contains(edge);
    }
}