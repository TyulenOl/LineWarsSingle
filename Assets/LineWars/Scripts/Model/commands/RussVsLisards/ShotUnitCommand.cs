using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitCommand<TNode, TEdge, TUnit> :
        ActionCommand<TUnit, IShotUnitAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly TNode targetNode;
        private readonly TUnit takenUnit;

        public ShotUnitCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode) : base(unit)
        {
            this.targetNode = targetNode;
            this.takenUnit = Action.TakenUnit;
        }

        public ShotUnitCommand(
            [NotNull] IShotUnitAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode targetNode) : base(action)
        {
            this.targetNode = targetNode;
            this.takenUnit = Action.TakenUnit;
        }

        public override void Execute()
        {
            Action.ShotUnitTo(targetNode);
        }

        public override bool CanExecute()
        {
            return Action.CanShotUnitTo(targetNode);
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} бросил юнита {takenUnit} в ноду {targetNode}";
        }
    }
}