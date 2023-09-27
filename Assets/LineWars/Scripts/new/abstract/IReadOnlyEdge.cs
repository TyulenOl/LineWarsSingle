using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IReadOnlyEdge: INumbered, IReadOnlyTarget
    {
        public int CurrentHp { get; }
        public LineType LineType { get; }

        public IReadOnlyNode FirstNode { get; }
        public IReadOnlyNode SecondNode { get; }
        
        public bool IsIncident(IReadOnlyNode node)
        {
            return FirstNode == node || FirstNode == node;
        }

        public IReadOnlyNode GetOther(IReadOnlyNode node)
        {
            if (!IsIncident(node)) throw new ArgumentException();
            return FirstNode.Equals(node) ? SecondNode : FirstNode;
        }
    }
}