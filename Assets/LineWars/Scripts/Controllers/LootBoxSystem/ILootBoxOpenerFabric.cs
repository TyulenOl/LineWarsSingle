using LineWars.Model;

namespace LineWars.Model
{
    public interface ILootBoxOpenerFabric
    {
        public ILootBoxOpener Create(LootBoxInfo info);
    }
}
