using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitHealCommand: ICommand
    {
        private readonly ComponentUnit.HealAction healAction;
        private readonly ComponentUnit doctor;
        private readonly ComponentUnit unit;
        
        public UnitHealCommand([NotNull] ComponentUnit doctor, [NotNull] ComponentUnit unit)
        {
            this.doctor = doctor ? doctor : throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ? unit : throw new ArgumentNullException(nameof(unit));
            
            healAction = doctor.TryGetExecutorAction<ComponentUnit.HealAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.HealAction)}");
        }

        public UnitHealCommand([NotNull] ComponentUnit.HealAction healAction, [NotNull] ComponentUnit unit)
        {
            this.healAction = healAction ?? throw new ArgumentNullException(nameof(healAction));
            this.unit = unit ? unit : throw new ArgumentNullException(nameof(unit));

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
            return $"Доктор {doctor.gameObject.name} похилил {unit.gameObject.name}";
        }
    }
}