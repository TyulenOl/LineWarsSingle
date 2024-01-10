using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class CardShower : LootedItemShower
    {
        [SerializeField] private CardDrawInfo cardDrawInfo;

        public void ShowItem(DeckCard deckCard)
        {
            cardDrawInfo.ReDraw(deckCard);
            cardDrawInfo.ReDrawAvailability(true);
        }
    }
}