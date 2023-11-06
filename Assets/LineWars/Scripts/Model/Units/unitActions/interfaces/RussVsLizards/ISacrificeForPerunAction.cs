namespace LineWars.Model
{
    public interface ISacrificeForPerunAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction

        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public bool CanSacrifice(TNode node);
        public void Sacrifice(TNode node);
    }
}