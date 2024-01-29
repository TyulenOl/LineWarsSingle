using LineWars.Model;

namespace LineWars.Model
{
    public interface ILootBoxOpener
    {
        public LootBoxInfo BoxInfo { get; }
        public  bool CanOpen(IReadOnlyUserInfo info);
        public  DropInfo Open(IReadOnlyUserInfo info);
    }
}
