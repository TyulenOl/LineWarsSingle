namespace LineWars.Model
{
    public class DiedUnit
    {
        public object Unit { get; }
        public DiedUnit(object unit)
        {
            Unit = unit;
        }   
    }
}