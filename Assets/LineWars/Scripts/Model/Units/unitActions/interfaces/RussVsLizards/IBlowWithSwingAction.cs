namespace LineWars.Model
{
    public interface IBlowWithSwingAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        IActionCommand ITargetedAction<TUnit>.GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit, IBlowWithSwingAction<TNode, TEdge, TUnit>, TUnit>
                (this, target);
        }
    }
}