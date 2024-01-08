using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class Deck : IDeck<DeckCard>
    {
        public int Id { get; }
        public string Name { get; set; }

        private readonly List<DeckCard> cards = new();
        public IReadOnlyList<DeckCard> Cards => cards;
        
        public void AddCard(DeckCard card)
        {
            cards.Add(card);
        }

        public void RemoveCard(DeckCard card)
        {
            cards.Remove(card);
        }

        public Deck(int id)
        {
            Id = id;
        }

        public Deck(int id, string name, IEnumerable<DeckCard> cards)
        {
            Id = id;
            Name = name;
            this.cards = cards.ToList();
        }
    }
}