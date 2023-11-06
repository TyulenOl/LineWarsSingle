using System;

namespace LineWars.Model
{
    public interface IRLBlockAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>, ISimpleAction

        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        bool IsBlocked { get; }
        public event Action<bool, bool> CanBlockChanged;
        
        public bool CanBlock();
        public void EnableBlock();
    }
}