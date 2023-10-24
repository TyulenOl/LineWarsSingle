namespace LineWars.Model
{
    public static class UnitExtension
    {
        public static int GetDamage(this Unit unit)
        {
            return unit.TryGetUnitAction<MonoAttackAction>(out var action)
                ? action.Damage
                : 0;
        }
    }
}