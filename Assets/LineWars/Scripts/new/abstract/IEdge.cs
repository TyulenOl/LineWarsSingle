using System;

namespace LineWars.Model
{
    public interface IEdge: IReadOnlyEdge, ITarget, IAlive
    {
        public new INode FirstNode { get; }
        public new INode SecondNode { get; }

        public bool IsIncident(INode node) => IsIncident((IReadOnlyNode) node);
        public INode GetOther(INode node) => (INode)GetOther((IReadOnlyNode) node);
    }
}