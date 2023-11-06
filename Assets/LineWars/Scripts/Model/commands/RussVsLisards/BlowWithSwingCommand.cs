using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlowWithSwingCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IBlowWithSwingAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;

        public BlowWithSwingCommand(
            [NotNull] TUnit unit) : this(
            unit.TryGetUnitAction<IBlowWithSwingAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IBlowWithSwingAction<TNode, TEdge, TUnit>)}"))
        {
        }

        public BlowWithSwingCommand(
            [NotNull] IBlowWithSwingAction<TNode, TEdge, TUnit> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            unit = action.MyUnit;
        }

        public void Execute()
        {
            action.ExecuteBlowWithSwing();
        }

        public bool CanExecute()
        {
            return action.CanBlowWithSwing();
        }

        public string GetLog()
        {
            return $"Юнит {unit} нанес круговую атаку";
        }

        public CommandType CommandType => action.CommandType;
    }
}