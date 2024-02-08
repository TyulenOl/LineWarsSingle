using System;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardBuyElementDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject bg;
        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text powerText;
        [SerializeField] private TMP_Text apText;
        [SerializeField] private Image sprite;
        
        
        [SerializeField] private Button button;
        [SerializeField] private CostDrawer costDrawer;

        public UnityEvent<DeckCard> CardOnClickEvent { get; private set; }
        public UnityEvent OnClickAction => button.onClick;
        
        private DeckCard deckCard;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        public void Redraw(DeckCard deckCard)
        {
            unitName.text = deckCard.Name;
            hpText.text = deckCard.Unit.MaxHp.ToString();
            powerText.text = deckCard.Unit.InitialPower.ToString();
            apText.text = deckCard.Unit.MaxActionPoints.ToString();
            sprite.sprite = deckCard.Image;
            
            costDrawer.DrawCost(deckCard.ShopCost, deckCard.ShopCostType);
            
            RedrawBg(DrawHelper.RarityToShopCardBg[deckCard.Rarity]);
            
            this.deckCard = deckCard;
        }

        private void RedrawBg(GameObject newBg)
        {
            if (bg != null)
                Destroy(bg.gameObject);
            var newBgInstance = Instantiate(newBg, transform);
            newBgInstance.transform.SetAsFirstSibling();
            bg = newBgInstance;
        }

        private void OnClick()
        {
            CardOnClickEvent?.Invoke(deckCard);
        }
    }
}