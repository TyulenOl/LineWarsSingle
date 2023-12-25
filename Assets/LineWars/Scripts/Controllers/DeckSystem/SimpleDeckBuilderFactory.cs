using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class SimpleDeckBuilderFactory : DeckBuilderFactory
    {
        [SerializeField, Min(0)] private int maxLargeUnits;
        [SerializeField, Min(0)] private int maxLittleUnits;

        public int MaxLargeUnits
        {
            get => maxLargeUnits;
            set => maxLargeUnits = value;
        }

        public int MaxLittleUnits
        {
            get => maxLittleUnits;
            set => maxLittleUnits = value;
        }

        public override IDeckBuilder<Deck, DeckCard> CreateNew()
        {
            return new SimpleDeckBuilder(maxLargeUnits, maxLittleUnits);
        }

        public override IDeckBuilder<Deck, DeckCard> CreateFromOtherDeck(Deck deck)
        {
            var builder = new SimpleDeckBuilder(maxLargeUnits, maxLittleUnits);

            foreach (var deckCard in deck.Cards.Where(x => x.Unit.Size == UnitSize.Large).Take(maxLargeUnits))
            {
                builder.AddCard(deckCard);
            }

            foreach (var deckCard in deck.Cards.Where(x => x.Unit.Size == UnitSize.Little).Take(maxLittleUnits))
            {
                builder.AddCard(deckCard);
            }

            return builder;
        }
    }
}