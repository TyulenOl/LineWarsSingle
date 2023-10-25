namespace LineWars.Model
{
    public class MonoShotUnitAction: MonoUnitAction,
        IShotUnitActon<Node, Edge, Unit, Owned, BasePlayer>
    {
        private ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer> Action
            => (ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        protected override ExecutorAction GetAction()
        {
            return new ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public Unit TakenUnit => Action.TakenUnit;
        
        public bool CanTakeUnit(Unit unit)
        {
            return Action.CanTakeUnit(unit);
        }

        public void TakeUnit(Unit unit)
        {
            Action.TakeUnit(unit);
        }

        public bool CanShotUnitTo(Node node)
        {
            return Action.CanShotUnitTo(node);
        }

        public void ShotUnit(Node node)
        {
            Action.ShotUnit(node);
        }
    }
}