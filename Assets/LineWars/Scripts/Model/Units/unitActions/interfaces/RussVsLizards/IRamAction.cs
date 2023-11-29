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
        
        bool ITargetedAction<TNode>.IsAvailable(TNode target) => CanRam(target);
        void ITargetedAction<TNode>.Execute(TNode target) => Ram(target);
        IActionCommand ITargetedAction<TNode>.GenerateCommand(TNode target)
        {
            return new RamCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}