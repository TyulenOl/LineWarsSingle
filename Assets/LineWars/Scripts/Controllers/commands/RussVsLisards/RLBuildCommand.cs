using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBuildCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
            ICommandWithCommandType

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        private readonly IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;
        private readonly TNode targetNode;
        private readonly BuildingType buildingType;

        public RLBuildCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode,
            BuildingType type) :
            this(unit.TryGetUnitAction<IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                    ? action
                    : throw new ArgumentException(
                        $"{nameof(TUnit)} does not contain {nameof(IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}"),
                targetNode, type)
        {
        }

        public RLBuildCommand(
            [NotNull] IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action,
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