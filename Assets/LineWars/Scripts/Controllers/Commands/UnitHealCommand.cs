using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitHealCommand: ICommand
    {
        private readonly ModelComponentUnit.HealAction healAction;
        private readonly ModelComponentUnit doctor;
        private readonly ModelComponentUnit unit;
        
        public UnitHealCommand([NotNull] ModelComponentUnit doctor, [NotNull] ModelComponentUnit unit)
        {
            this.doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            
            healAction = doctor.TryGetExecutorAction<ModelComponentUnit.HealAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.HealAction)}");
        }

        public UnitHealCommand([NotNull] ModelComponentUnit.HealAction healAction, [NotNull] ModelComponentUnit unit)
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