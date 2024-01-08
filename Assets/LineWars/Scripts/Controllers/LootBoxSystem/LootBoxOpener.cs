using LineWars.Model;

namespace LineWars.LootBoxes
{
    public interface LootBoxOpener
    {
        public LootBoxInfo BoxInfo { get; }
        public  bool CanOpen(UserInfo info);
        public  DropInfo Open(UserInfo info);
    }
}
