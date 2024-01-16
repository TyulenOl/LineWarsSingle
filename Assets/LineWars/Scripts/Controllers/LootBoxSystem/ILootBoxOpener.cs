using LineWars.Model;

namespace LineWars.LootBoxes
{
    public interface ILootBoxOpener
    {
        public LootBoxInfo BoxInfo { get; }
        public  bool CanOpen(UserInfo info);
        public  DropInfo Open(UserInfo info);
    }
}
