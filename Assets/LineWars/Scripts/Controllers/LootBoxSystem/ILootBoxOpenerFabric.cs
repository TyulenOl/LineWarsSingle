namespace LineWars.LootBoxes
{
    public interface ILootBoxOpenerFabric
    {
        public ILootBoxOpener Create(LootBoxInfo info);
    }
}
