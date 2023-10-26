using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
            ICommandWithCommandType

        #region Сonstraints

        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>

    #endregion

    {
        private readonly IShotUnitActon<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;
        private readonly TNode targetNode;
        private readonly TUnit takenUnit;

        public ShotUnitCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode) :
            this(unit.TryGetUnitAction<IShotUnitActon<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                    ? action
                    : throw new ArgumentException(
                        $"{nameof(TUnit)} does not contain {nameof(IShotUnitActon<TNode, TEdge, TUnit, TOwned, TPlayer>)}"),
                targetNode)
        {
        }

        public ShotUnitCommand(
            [NotNull] IShotUnitActon<TNode, TEdge, TUnit, TOwned, TPlayer> action,
            [NotNull] TNode targetNode)
        {
            this.action = action;
            unit = action.MyUnit;
            this.targetNode = targetNode;
            this.takenUnit = action.TakenUnit;
        }

        public void Execute()
        {
            action.ShotUnitTo(targetNode);
        }

        public bool CanExecute()
        {
            return action.CanShotUnitTo(targetNode);
        }

        public string GetLog()
        {
            return $"Юнит {unit} бросил юнита {takenUnit} в ноду {targetNode}";
        }

        public CommandType CommandType => action.CommandType;
    }
}