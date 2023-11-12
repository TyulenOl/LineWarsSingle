using System;

namespace LineWars.Model
{
    public class MonoShotUnitAction :
        MonoUnitAction<ShotUnitAction<Node, Edge, Unit>>,
        IShotUnitAction<Node, Edge, Unit>
    {
        public bool IsAvailable(Unit target1) => Action.IsAvailable(target1);

        public bool IsAvailable(Unit target1, Node target2) => Action.IsAvailable(target1, target2);

        public void Execute(Unit target1, Node target2)
        {
            target1.MovementLogic.MoveTo(target2.transform);
            Action.Execute(target1, target2);
            Player.LocalPlayer.RecalculateVisibility();
        }

        protected override ShotUnitAction<Node, Edge, Unit> GetAction()
        {
            return new ShotUnitAction<Node, Edge, Unit>(Unit);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}