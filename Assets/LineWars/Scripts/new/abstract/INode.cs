using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INode : IReadOnlyNode, ITarget, IOwned
    {
        public new IUnit LeftUnit { get; set; }
        public new IUnit RightUnit { get; set; }
        public new IReadOnlyCollection<IEdge> Edges { get; }
        public IEdge GetLine(INode node) => (IEdge) GetLine((IReadOnlyNode) node);

        public new IEnumerable<INode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }
    }
}