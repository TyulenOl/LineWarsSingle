using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class UpgradeCardShower : ItemWithAmountShower
    {
        [SerializeField] private CardDrawInfo cardDrawInfo;

        public void ShowItem(DeckCard deckCard,int amount)
        {
            cardDrawInfo.ReDraw(deckCard);
            cardDrawInfo.ReDrawAvailability(true);
            base.ShowItem(amount);
        }
    }
}