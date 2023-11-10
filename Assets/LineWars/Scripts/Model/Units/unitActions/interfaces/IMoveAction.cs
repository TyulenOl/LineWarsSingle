using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IMoveAction<TNode, TEdge, TUnit>:
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TNode>

        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        bool CanMoveTo([NotNull] TNode target, bool ignoreActionPointsCondition = false);
        void MoveTo([NotNull] TNode target);
        
        
        bool ITargetedAction<TNode>.IsAvailable(TNode target) => CanMoveTo(target);
        void ITargetedAction<TNode>.Execute(TNode target) => MoveTo(target);
        IActionCommand ITargetedAction<TNode>.GenerateCommand(TNode target)
        {
            return new MoveCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}