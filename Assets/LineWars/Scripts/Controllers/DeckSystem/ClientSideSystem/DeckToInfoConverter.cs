using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public class DeckToInfoConverter
        : IConverter<Deck, DeckInfo>
    {
        private readonly IReadOnlyDictionary<DeckCard, int> cardToId;
        public DeckToInfoConverter(IReadOnlyDictionary<DeckCard, int> cardToId) 
        { 
            this.cardToId = cardToId;
        }

        public DeckInfo Convert(Deck value)
        {
            var newDeckInfo = new DeckInfo();
            newDeckInfo.Name = value.Name;
            foreach(var card in value.Cards)
            {
                var newCardInfo = new DeckCardInfo();
                newCardInfo.CardId = cardToId[card];
                newDeckInfo.Cards.Add(newCardInfo);
            }

            return newDeckInfo;
        }
    }
}
