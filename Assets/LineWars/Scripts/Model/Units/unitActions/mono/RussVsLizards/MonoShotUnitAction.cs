using System;

namespace LineWars.Model
{
    public class MonoShotUnitAction :
        MonoUnitAction<ShotUnitAction<Node, Edge, Unit>>,
        IShotUnitAction<Node, Edge, Unit>
    {
        protected override ShotUnitAction<Node, Edge, Unit> GetAction()
        {
            return new ShotUnitAction<Node, Edge, Unit>(Unit);
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
                return new TakeUnitCommand<Node, Edge, Unit>(this, (Unit) target);
            }

            return new ShotUnitCommand<Node, Edge, Unit>(this, (Node) target);
        }
        
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}