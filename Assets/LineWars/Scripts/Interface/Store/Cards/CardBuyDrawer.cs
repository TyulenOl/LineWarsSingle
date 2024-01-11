using System;
using LineWars.LootBoxes;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars
{
    public class CardBuyDrawer : MonoBehaviour
    {
        [SerializeField] private CardDrawInfo cardDrawInfo;
        [SerializeField] private Button button;
        [SerializeField] private CostDrawer costDrawer;
        
        public void Init(DeckCard deckCard, UnityAction onButtonClickAction = null)
        {
            if(onButtonClickAction != null)
                button.onClick.AddListener(onButtonClickAction);
            cardDrawInfo.ReDraw(deckCard);
            cardDrawInfo.ReDrawAvailability(true);
            costDrawer.DrawCost(deckCard.ShopCost, deckCard.ShopCostType);
        }
    }
}