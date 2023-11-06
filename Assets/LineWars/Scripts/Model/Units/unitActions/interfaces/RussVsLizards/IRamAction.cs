namespace LineWars.Model
{
    public interface IRamAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TNode>,
        IActionWithDamage
        
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public bool CanRam(TNode node);
        public void Ram(TNode node);
    }
}