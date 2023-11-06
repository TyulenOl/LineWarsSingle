namespace LineWars.Model
{
    public interface IMonoUnitAction<out TAction> :
        IMonoExecutorAction<Unit, TAction>,
        IUnitAction<Node, Edge, Unit>
        where TAction : UnitAction<Node, Edge, Unit>
    {
        public void Accept(IMonoUnitVisitor visitor);
    }
}