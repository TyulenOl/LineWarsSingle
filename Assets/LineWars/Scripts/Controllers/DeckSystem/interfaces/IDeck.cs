
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IDeck<TDeckCard>
        where TDeckCard : IDeckCard
    {
        public string Name { get; }
        public IReadOnlyList<TDeckCard> Cards { get; }

        public void AddCard(TDeckCard card);
        public bool RemoveCard(TDeckCard card);
    }
}
