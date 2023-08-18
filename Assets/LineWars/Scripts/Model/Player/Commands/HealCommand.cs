namespace LineWars.Model
{
    public class HealCommand: ICommand
    {
        private readonly IDoctor doctor;
        private readonly Unit unit;
        
        public HealCommand(IDoctor doctor, Unit unit)
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
    }
}