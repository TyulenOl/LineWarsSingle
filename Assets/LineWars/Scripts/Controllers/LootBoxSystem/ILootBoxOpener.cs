using LineWars.Model;

namespace LineWars.LootBoxes
{
    public interface ILootBoxOpener
    {
        public LootBoxInfo BoxInfo { get; }
        public  bool CanOpen(IReadOnlyUserInfo info);
        public  DropInfo Open(IReadOnlyUserInfo info);
    }
}
