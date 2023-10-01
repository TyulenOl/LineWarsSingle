using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        ICommand
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly HealAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> healAction;
        private readonly TUnit doctor;
        private readonly TUnit unit;

        public HealCommand([NotNull] TUnit doctor, [NotNull] TUnit unit)
        {
            this.doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));

            healAction = doctor.TryGetUnitAction<HealAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(HealAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>)}");
        }

        public HealCommand([NotNull] HealAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> healAction, [NotNull] TUnit unit)
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
