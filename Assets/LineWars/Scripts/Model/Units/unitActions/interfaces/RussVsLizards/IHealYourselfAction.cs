namespace LineWars.Model
{
    public interface IHealYourselfAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ISimpleAction
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        int HealAmount { get; }
        
        CommandType IExecutorAction.CommandType => CommandType.VodaBajkalskaya;
        IActionCommand ISimpleAction.GenerateCommand()
        {
            return new HealYourSelfCommand<TNode, TEdge, TUnit>(this);
        }
    }
}