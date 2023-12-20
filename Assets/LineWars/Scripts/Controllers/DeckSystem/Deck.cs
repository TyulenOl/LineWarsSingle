using System.Collections.Generic;

namespace LineWars.Model
{
    public class Deck : IDeck<DeckCard>
    {
        private List<DeckCard> cards = new();
        public string Name { get; set; }
        public IReadOnlyList<DeckCard> Cards => cards;

        public void AddCard(DeckCard card)
        {
            cards.Add(card);
        }

        public bool RemoveCard(DeckCard card)
        {
            return cards.Remove(card);
        }
    }
}
