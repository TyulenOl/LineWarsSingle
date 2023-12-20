using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IAllCards
    {
        public IReadOnlyDictionary<int, DeckCard> IdToCard { get; }
        public IReadOnlyDictionary<DeckCard, int> CardToId { get; }
    }
}
