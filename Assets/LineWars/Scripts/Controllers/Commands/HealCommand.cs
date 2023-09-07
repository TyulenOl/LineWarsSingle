namespace LineWars.Model
{
    public class HealCommand: ICommand
    {
        private readonly Doctor doctor;
        private readonly Unit unit;
        
        public HealCommand(Doctor doctor, Unit unit)
        {
            this.doctor = doctor;
            this.unit = unit;
        }
        public void Execute()
        { 
            doctor.Heal(unit);
        }

        public bool CanExecute()
        {
            return doctor.CanHeal(unit);
        }

        public string GetLog()
        {
            return $"Доктор {doctor.gameObject.name} похилил {unit.gameObject.name}";
        }
    }
}