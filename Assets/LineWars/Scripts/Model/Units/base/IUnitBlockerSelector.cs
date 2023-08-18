namespace LineWars.Model
{
    public interface IUnitBlockerSelector
    {
        public Unit SelectBlocker(Unit targetUnit, Unit neighborUnit);
    }
}