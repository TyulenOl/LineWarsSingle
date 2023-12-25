using UnityEngine;

namespace LineWars.Model
{
    public abstract class DeckBuilderFactory: MonoBehaviour
    {
        public abstract IDeckBuilder<Deck, DeckCard> CreateNew();
        public abstract IDeckBuilder<Deck, DeckCard> CreateFromOtherDeck(Deck deck);
    }
}