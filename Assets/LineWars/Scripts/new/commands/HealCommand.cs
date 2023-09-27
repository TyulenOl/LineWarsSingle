using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealCommand : ICommand
    {
        private readonly HealAction healAction;
        private readonly IUnit doctor;
        private readonly IUnit unit;

        public HealCommand([NotNull] IUnit doctor, [NotNull] IUnit unit)
        {
            this.doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));

            healAction = doctor.TryGetExecutorAction<HealAction>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(IUnit)} does not contain {nameof(HealAction)}");
        }

        public HealCommand([NotNull] HealAction healAction, [NotNull] IUnit unit)
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
