using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class Deck : IDeck<DeckCard>, IEquatable<Deck>
    {
        public int Id { get; }
        public string Name { get; set; }

        private readonly HashSet<DeckCard> cards = new();
        public IReadOnlyCollection<DeckCard> Cards => cards;
        
        public Deck(int id)
        {
            Id = id;
        }

        public Deck(int id, string name, IEnumerable<DeckCard> cards)
        {
            Id = id;
            Name = name;
            this.cards = cards
                .Where(x => x != null)
                .ToHashSet();
        }
        
        public void AddCard([NotNull] DeckCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            cards.Add(card);
        }

        public void RemoveCard(DeckCard card)
        {
            cards.Remove(card);
        }

        public bool Equals(Deck other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return cards.SetEquals(other.cards) && Id == other.Id && Name == other.Name;
        }
    }
}