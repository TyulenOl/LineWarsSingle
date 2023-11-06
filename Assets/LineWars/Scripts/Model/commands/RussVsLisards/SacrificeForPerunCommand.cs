using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class SacrificeForPerunCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly ISacrificeForPerunAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;
        private readonly TNode node;

        public SacrificeForPerunCommand([NotNull] TUnit unit, [NotNull] TNode node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.action = unit.TryGetUnitAction<ISacrificeForPerunAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(ISacrificeForPerunAction<TNode, TEdge, TUnit>)}");
        }

        public SacrificeForPerunCommand(
            [NotNull] ISacrificeForPerunAction<TNode, TEdge, TUnit> sacrificeForAction,
            [NotNull] TNode node)
        {
            this.action = sacrificeForAction ?? throw new ArgumentNullException(nameof(sacrificeForAction));
            unit = sacrificeForAction.MyUnit;
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public void Execute() => action.Sacrifice(node);
        public bool CanExecute() => action.CanSacrifice(node);

        public string GetLog()
        {
            return
                $"Юнит {unit} пожертвовал собой ради того, чтобы нанести урон всем в юнитам в ноде {node}. Настоящий герой!";
        }

        public CommandType CommandType => action.CommandType;
    }
}