

namespace LineWars.Model
{
    public static class UnitProjectionCreator
    {
        public static UnitProjection FromProjection(
            IReadOnlyUnitProjection oldUnit,
            NodeProjection node = null)
        {
            var newUnit = new UnitProjection
            {
                UnitName = oldUnit.UnitName,
                CurrentHp = oldUnit.CurrentHp,
                MaxHp = oldUnit.MaxHp,
                MaxArmor = oldUnit.MaxArmor,
                CurrentArmor = oldUnit.CurrentArmor,
                CurrentActionPoints = oldUnit.CurrentActionPoints,
                MaxActionPoints = oldUnit.MaxActionPoints,
                Visibility = oldUnit.Visibility,
                Type = oldUnit.Type,
                Size = oldUnit.Size,
                MovementLineType = oldUnit.MovementLineType,
                CommandPriorityData = oldUnit.CommandPriorityData,
                UnitDirection = oldUnit.UnitDirection,
                Node = node,
                Original = oldUnit.Original,
                UnitActions = oldUnit.ActionsDictionary.Values,
                HasId = oldUnit.HasId,
                Id = oldUnit.Id
            };

            return newUnit;
        }

        public static UnitProjection FromMono(
            Unit original,
            NodeProjection node = null)
        {
            var newUnit = new UnitProjection
            {
                UnitName = original.UnitName,
                CurrentHp = original.CurrentHp,
                MaxHp = original.MaxHp,
                MaxArmor = original.MaxArmor,
                CurrentArmor = original.CurrentArmor,
                CurrentActionPoints = original.CurrentActionPoints,
                MaxActionPoints = original.MaxActionPoints,
                Visibility = original.Visibility,
                Type = original.Type,
                Size = original.Size,
                MovementLineType = original.MovementLineType,
                CommandPriorityData = original.CommandPriorityData,
                UnitDirection = original.UnitDirection,
                Node = node,
                Original = original,
                MonoActions = original.MonoActions,
                HasId = true,
                Id = original.Id
            };

            return newUnit;
        }
    }
}
