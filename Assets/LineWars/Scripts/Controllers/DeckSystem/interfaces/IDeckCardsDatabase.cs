using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IDeckCardsDatabase
    {
        public IReadOnlyDictionary<int, DeckCard> IdToCard { get; }
        public IReadOnlyDictionary<DeckCard, int> CardToId { get; }
        public IEnumerable<DeckCard> AllCards { get; }
        public int CardsCount { get; }
    }
}
