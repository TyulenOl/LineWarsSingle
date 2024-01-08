namespace LineWars.LootBoxes
{
    public interface ILootBoxOpenerFabric
    {
        public LootBoxOpener Create(LootBoxInfo info);
    }
}
