using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class DeckToInfoConverter : IConverter<Deck, DeckInfo>
    {
        private readonly IReadOnlyDictionary<DeckCard, int> cardToId;

        public DeckToInfoConverter([NotNull] IReadOnlyDictionary<DeckCard, int> cardToId)
        {
            this.cardToId = cardToId ?? throw new ArgumentNullException(nameof(cardToId));
        }

        public DeckInfo Convert(Deck value)
        {
            return new DeckInfo
            {
                Id = value.Id,
                Name = value.Name,
                Cards = value.Cards
                    .Select(e => new DeckCardInfo()
                    {
                        CardId = cardToId[e]
                    })
                    .ToList()
            };;
        }
    }
}