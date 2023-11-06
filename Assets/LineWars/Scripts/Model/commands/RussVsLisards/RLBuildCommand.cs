using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBuildCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IRLBuildAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;
        private readonly TNode targetNode;
        private readonly BuildingType buildingType;

        public RLBuildCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode,
            BuildingType type) :
            this(unit.TryGetUnitAction<IRLBuildAction<TNode, TEdge, TUnit>>(out var action)
                    ? action
                    : throw new ArgumentException(
                        $"{nameof(TUnit)} does not contain {nameof(IRLBuildAction<TNode, TEdge, TUnit>)}"),
                targetNode, type)
        {
        }

        public RLBuildCommand(
            [NotNull] IRLBuildAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode targetNode,
            BuildingType type)
        {
            this.action = action;
            unit = action.MyUnit;
            this.targetNode = targetNode;
            buildingType = type;
        }

        public void Execute()
        {
            action.Build(targetNode, buildingType);
        }

        public bool CanExecute()
        {
            return action.CanBuild(targetNode, buildingType);
        }

        public string GetLog()
        {
            return $"Юнит {unit} постоил строение типа {buildingType} в ноде {targetNode}";
        }

        public CommandType CommandType => action.CommandType;
    }
}