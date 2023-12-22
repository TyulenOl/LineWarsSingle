using System.Collections.Generic;

namespace LineWars.Model
{
    public class DeckInfoToDeckConverter : IConverter<DeckInfo, Deck>
    {
        private readonly IReadOnlyDictionary<int, DeckCard> idToCard;

        public DeckInfoToDeckConverter(IReadOnlyDictionary<int, DeckCard> idToCard)
        {
            this.idToCard = idToCard;
        }

        public Deck Convert(DeckInfo value)
        {
            var deck = new Deck();
            deck.Name = value.Name;
            foreach (var card in value.Cards)
            {
                var newCard = idToCard[card.CardId];
                deck.AddCard(newCard);
            }

            return deck;
        }
    }
}
