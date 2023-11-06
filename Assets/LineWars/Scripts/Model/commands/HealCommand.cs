using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IHealAction<TNode, TEdge, TUnit> healAction;
        private readonly TUnit doctor;
        private readonly TUnit unit;

        public HealCommand([NotNull] TUnit doctor, [NotNull] TUnit unit)
        {
            this.doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));

            healAction = doctor.TryGetUnitAction<IHealAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IHealAction<TNode, TEdge, TUnit>)}");
        }

        public HealCommand(
            [NotNull] IHealAction<TNode, TEdge, TUnit> healAction,
            [NotNull] TUnit unit)
        {
            this.healAction = healAction ?? throw new ArgumentNullException(nameof(healAction));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));

            doctor = healAction.MyUnit;
        }

        public void Execute()
        {
            healAction.Heal(unit);
        }

        public bool CanExecute()
        {
            return healAction.CanHeal(unit);
        }

        public string GetLog()
        {
            return $"Доктор {doctor} похилил {unit}";
        }

        public CommandType CommandType => healAction.CommandType;
    }
}