namespace LineWars.Model
{
    public class MonoBuildRoadAction: MonoUnitAction
    {
        protected override UnitAction GetAction(ComponentUnit unit) => new BuildAction(unit, this);
    }
}