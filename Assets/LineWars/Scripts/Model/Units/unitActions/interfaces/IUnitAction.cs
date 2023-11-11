namespace LineWars.Model
{
    public interface IUnitAction<TNode, TEdge, TUnit> :
        IExecutorAction<TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public TUnit MyUnit => Executor;
        public uint GetPossibleMaxRadius();

        public TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor);
    }
}