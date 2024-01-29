namespace LineWars.Model
{
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