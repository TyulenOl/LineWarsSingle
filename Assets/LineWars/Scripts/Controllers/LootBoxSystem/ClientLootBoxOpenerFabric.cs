using LineWars.Model;

namespace LineWars.LootBoxes
{
    public class ClientLootBoxOpenerFabric : ILootBoxOpenerFabric
    {
        private readonly IStorage<int, DeckCard> cardStorage;

        public ClientLootBoxOpenerFabric(IStorage<int, DeckCard> cardStorage)
        {
            this.cardStorage = cardStorage;
        }

        public ILootBoxOpener Create(LootBoxInfo info)
        {
            return new ClientLootBoxOpener(info, cardStorage);
        }
    }
}
