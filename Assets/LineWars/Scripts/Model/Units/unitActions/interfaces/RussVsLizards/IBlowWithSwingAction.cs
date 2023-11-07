namespace LineWars.Model
{
    public interface IBlowWithSwingAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ISimpleAction,
        IActionWithDamage
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public bool CanBlowWithSwing();
        public void ExecuteBlowWithSwing();

        
        bool ISimpleAction.CanExecute() => CanBlowWithSwing();
        void ISimpleAction.Execute() => ExecuteBlowWithSwing();
        IActionCommand ISimpleAction.GenerateCommand()
        {
            return new BlowWithSwingCommand<TNode, TEdge, TUnit>(this);
        }
    }
}