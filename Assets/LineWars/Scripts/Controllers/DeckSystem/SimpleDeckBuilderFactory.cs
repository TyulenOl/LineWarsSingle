using UnityEngine;

namespace LineWars.Model
{
    public class SimpleDeckBuilderFactory: DeckBuilderFactory
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
    }
}