namespace LineWars.Model
{
    public interface ISacrificeForPerunAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public bool CanSacrifice(TNode node);
        public void Sacrifice(TNode node);

        
        bool ITargetedAction<TNode>.IsAvailable(TNode target) => CanSacrifice(target);
        void ITargetedAction<TNode>.Execute(TNode target) => Sacrifice(target);

        IActionCommand ITargetedAction<TNode>.GenerateCommand(TNode target)
        {
            return new SacrificeForPerunCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}