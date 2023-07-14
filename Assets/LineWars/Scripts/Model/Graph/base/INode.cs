using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public interface INode
    {
        public Vector2 Position { get; }
        public IReadOnlyCollection<IEdge> Edges { get; }

        public IEnumerable<INode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirsNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirsNode;
            }
        }
    }
}