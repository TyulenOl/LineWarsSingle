using System.Collections.Generic;
using System.Linq;

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
            if (value == null)
                return null;
            
            return new Deck(
                value.Id,
                value.Name,
                value.Cards
                .Where(card => idToCard.ContainsKey(card.CardId))
                .Select(e => idToCard[e.CardId])
            );
        }
    }
}