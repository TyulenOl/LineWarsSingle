namespace LineWars.Model
{
    public interface IJumpAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public int MinJumpDistance { get; }
        public int MaxJumpDistance { get; }
    }
}
