using LineWars.Model;
using System.Collections.Generic;

namespace LineWars.Controllers
{
    public static class CardHelper
    {
        public static IEnumerable<int> FindCardsByType(this IStorage<int,DeckCard> storage, Rarity rarity)
        {
            foreach (var card in storage.Values)
            {
                if (card.Rarity == rarity)
                    yield return storage.ValueToId[card];
            }
        }
    }
}
