using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class Deck : IDeck<DeckCard>
    {
        public int Id { get; }
        public string Name { get;}

        private readonly List<DeckCard> cards;
        public IReadOnlyList<DeckCard> Cards => cards;

        public Deck(int id, string name, IEnumerable<DeckCard> cards)
        {
            Id = id;
            Name = name;
            this.cards = cards.ToList();
        }
    }
}