namespace LineWars.Model
{
    public interface IShotUnitAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        IMultiTargetedAction<TUnit, TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        IActionCommand IMultiTargetedAction<TUnit, TNode>.GenerateCommand(TUnit unit, TNode node)
        {
            return new ShotUnitCommand<TNode, TEdge, TUnit>(this, unit, node);
        }
    }
}