namespace LineWars.Model
{
    public class UnitHealCommand: ICommand
    {
        private readonly ComponentUnit doctor;
        private readonly ComponentUnit unit;
        
        public UnitHealCommand(ComponentUnit doctor, ComponentUnit unit)
        {
            this.doctor = doctor;
            this.unit = unit;
        }
        public void Execute()
        { 
            doctor.GetExecutorAction<ComponentUnit.HealAction>().Heal(unit);
        }

        public bool CanExecute()
        {
            return doctor.TryGetExecutorAction<ComponentUnit.HealAction>(out var action)
                   && action.CanHeal(unit);
        }

        public string GetLog()
        {
            return $"Доктор {doctor.gameObject.name} похилил {unit.gameObject.name}";
        }
    }
}