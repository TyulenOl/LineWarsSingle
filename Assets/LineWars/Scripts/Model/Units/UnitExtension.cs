namespace LineWars.Model
{
    public static class UnitExtension
    {
        public static int GetDamage(this Unit unit)
        {
            return unit.TryGetUnitAction<IAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>>(out var action)
                ? action.Damage
                : 0;
        }
    }
}