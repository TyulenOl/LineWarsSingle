using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
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