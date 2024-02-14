using LineWars.Model;

namespace LineWars.Model
{
    public class ClientLootBoxOpenerFabric : ILootBoxOpenerFabric
    {
        private readonly IStorage<int, DeckCard> cardStorage;
        private readonly IStorage<BlessingId, BaseBlessing> blessingStorage;

        public ClientLootBoxOpenerFabric(
            IStorage<int, DeckCard> cardStorage,
            IStorage<BlessingId, BaseBlessing> blessingStorage)
        {
            this.cardStorage = cardStorage;
            this.blessingStorage = blessingStorage;
        }

        public ILootBoxOpener Create(LootBoxInfo info)
        {
            return new ClientLootBoxOpener(info, cardStorage, blessingStorage);
        }
    }
}
