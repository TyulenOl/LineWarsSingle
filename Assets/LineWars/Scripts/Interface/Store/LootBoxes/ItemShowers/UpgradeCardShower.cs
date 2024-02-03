using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class UpgradeCardShower : ItemWithAmountShower
    {
        [SerializeField] private BaseCardDrawer baseCardDrawer;
        //[SerializeField] private CardDrawInfo cardDrawInfo;

        public void ShowItem(DeckCard deckCard,int amount)
        {
            baseCardDrawer.DeckCard = deckCard;
            
            // cardDrawInfo.ReDraw(deckCard);
            // cardDrawInfo.ReDrawAvailability(true);
            base.ShowItem(amount);
        }
    }
}