using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealCommand<TNode, TEdge, TUnit, TOwned, TPlayer>:
        ICommand
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        private readonly IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer> healAction;
        private readonly TUnit doctor;
        private readonly TUnit unit;

        public HealCommand([NotNull] TUnit doctor, [NotNull] TUnit unit)
        {
            this.doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));

            healAction = doctor.TryGetUnitAction<IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}");
        }

        public HealCommand(
            [NotNull] IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer> healAction,
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
    }
}
