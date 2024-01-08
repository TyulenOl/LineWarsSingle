using System.Collections.Generic;

namespace LineWars.LootBoxes
{
    public class DropInfo
    {
        public readonly IReadOnlyList<Drop> Drops;

        public DropInfo(IReadOnlyList<Drop> drops)
        {
            Drops = drops;
        }
    }

    public class Drop
    {
        public readonly LootType DropType;
        public readonly int Value;

        public Drop(LootType dropType, int value)
        {
            DropType = dropType;
            Value = value;
        }
    }
}
