using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBuildCommand<TNode, TEdge, TUnit> :
        ActionCommand<TUnit, IRLBuildAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly TNode targetNode;
        private readonly BuildingType buildingType;

        public RLBuildCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode,
            BuildingType type) : base(unit)
        {
            this.targetNode = targetNode;
            buildingType = type;
        }

        public RLBuildCommand(
            [NotNull] IRLBuildAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode targetNode,
            BuildingType type): base(action)
        {
            this.targetNode = targetNode;
            buildingType = type;
        }

        public override void Execute()
        {
            Action.Build(targetNode, buildingType);
        }

        public override bool CanExecute()
        {
            return Action.CanBuild(targetNode, buildingType);
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} постоил строение типа {buildingType} в ноде {targetNode}";
        }
    }
}