namespace LineWars.Model
{
    public interface IMonoUnitAction<out TAction> :
        IMonoExecutorAction<Unit, TAction>,
        IUnitAction<Node, Edge, Unit, Owned, BasePlayer>
        where TAction : UnitAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        public void Accept(IMonoUnitVisitor visitor);
    }
}