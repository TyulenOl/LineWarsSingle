using System.Diagnostics.CodeAnalysis;
using LineWars.Model.unitActions;

namespace LineWars.Model
{
    public interface IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        bool CanMoveTo([NotNull] TNode target, bool ignoreActionPointsCondition = false);
        void MoveTo([NotNull] TNode target);
    }
}