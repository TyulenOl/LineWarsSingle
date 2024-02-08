namespace LineWars.Model
{
    public interface IUnitAction: IExecutorAction
    {
        
    }
    public interface IUnitAction<TNode, TEdge, TUnit> :
        IUnitAction,
        IExecutorAction<TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor);
    }
}