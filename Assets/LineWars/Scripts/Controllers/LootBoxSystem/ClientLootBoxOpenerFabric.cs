using LineWars.Model;

namespace LineWars.LootBoxes
{
    public class ClientLootBoxOpenerFabric : ILootBoxOpenerFabric
    {
        private readonly IStorage<DeckCard> cardStorage;

        public ClientLootBoxOpenerFabric(IStorage<DeckCard> cardStorage)
        {
            this.cardStorage = cardStorage;
        }

        public LootBoxOpener Create(LootBoxInfo info)
        {
            return new ClientLootBoxOpener(info, cardStorage);
        }
    }
}
