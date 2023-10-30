using System;

namespace LineWars.Model
{
    public class MonoShotUnitAction :
        MonoUnitAction<ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IShotUnitAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        protected override ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
        }
        
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

        public void ShotUnitTo(Node node)
        {
            Action.ShotUnitTo(node);
        }

        public Type TargetType => Action.TargetType;
        public Type[] MyTargets => Action.MyTargets;
        public bool IsMyTarget(ITarget target) => Action.IsMyTarget(target);

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            if (TakenUnit == null)
            {
                return new TakeUnitCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Unit) target);
            }

            return new ShotUnitCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Node) target);
        }
        
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}