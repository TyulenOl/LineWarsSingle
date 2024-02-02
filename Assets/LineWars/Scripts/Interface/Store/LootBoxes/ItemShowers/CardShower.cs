using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class CardShower : LootedItemShower
    {
        [SerializeField] private BaseCardDrawer baseCardDrawer;

        public void ShowItem(DeckCard deckCard)
        {
            baseCardDrawer.DeckCard = deckCard;
        }
    }
}