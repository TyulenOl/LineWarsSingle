using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.LootBoxes;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class CardsStoreDrawer : MonoBehaviour
    {
        [SerializeField] private CardBuyDrawer cardDrawInfoPrefab;
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private LayoutGroup boxesLayout;
        [SerializeField] private BoxImagesShower boxImagesShower;

        private void Awake()
        {
            ReDrawBoxes();
        }

        private void ReDrawBoxes()
        {
            var cards = GameRoot.Instance.CardStore.CardsForPurchase.Select(x => GameRoot.Instance.CardsDatabase.IdToValue[x]);
            foreach (var deckCard in cards)
            {
                var instance = Instantiate(cardDrawInfoPrefab, boxesLayout.transform);
                instance.Init(deckCard, () => buyPanel.OpenWindow(GetBuyPanelReDrawInfo(deckCard)));
            }
        }
        
        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo(DeckCard deckCard)
        {
            return new BuyPanelReDrawInfo(() => OnButtonClick(deckCard),
                () => GameRoot.Instance.CardStore.CanBuy(GameRoot.Instance.CardsDatabase.ValueToId[deckCard]),
                deckCard.Image,
                deckCard.Name,
                deckCard.Description,
                deckCard.ShopCost,
                deckCard.ShopCostType);
        }
        
        private void OnButtonClick(DeckCard deckCard)
        {
            var cardId = GameRoot.Instance.CardsDatabase.ValueToId[deckCard];
            
            if (!GameRoot.Instance.CardStore.CanBuy(cardId))
                throw new InvalidOperationException("Can't buy this Card");
            GameRoot.Instance.CardStore.Buy(cardId);

            var contextedDropArray = new List<ContextedDrop>
            {
                new (new Drop(LootType.Card, cardId))
            };
            
            boxImagesShower.gameObject.SetActive(true);
            boxImagesShower.ShowItems(contextedDropArray);
        }
    }
}
