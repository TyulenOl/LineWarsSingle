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
            return new Deck(
                value.Id,
                value.Name,
                value.Cards.Select(e => idToCard[e.CardId])
            );
        }
    }
}