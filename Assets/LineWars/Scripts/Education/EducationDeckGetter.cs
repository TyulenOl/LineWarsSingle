using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class EducationDeckGetter: DeckGetter
    {
        [SerializeField] private List<DeckCard> deckCards;
        public override bool CanGet()
        {
            return true;
        }

        public override Deck Get()
        {
            var deck = new Deck(0)
            {
                Name = "EducationDeck"
            };
            foreach (var card in deckCards)
            {
                card.Level = 0;
                deck.AddCard(card);
            }
                
            
            return deck;
        }
    }
}