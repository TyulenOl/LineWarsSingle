namespace LineWars.Model
{
    public class Drop
    {
        public readonly LootType DropType;
        public readonly int Value;
        public readonly BlessingId BlessingId;

        public Drop(LootType dropType, int value)
        {
            DropType = dropType;
            Value = value;
        }

        public Drop(LootType dropType, BlessingId blessingId, int count)
        {
            DropType = dropType;
            Value = count;
            BlessingId = blessingId;
        }
    }
}