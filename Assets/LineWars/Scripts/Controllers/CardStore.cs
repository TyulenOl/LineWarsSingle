using LineWars.Model;
using LineWars.Controllers;
using LineWars.LootBoxes;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Store
{
    public class CardStore : MonoBehaviour
    {
        [SerializeField] private List<CardRarity> cards;
        
        private IGetter<DateTime> timeGetter;
        private IStorage<DeckCard> cardStorage;
        private UserInfoController userInfoController;

        private bool isStoreInitialized = false;
        private List<int> cardsForPurchase = new();
        public bool StoreInitialized => isStoreInitialized;
        public IReadOnlyList<int> CardsForPurchase => cardsForPurchase;

        public void Initialize(
            IGetter<DateTime> timeGetter, 
            IStorage<DeckCard> cardStorage,
            UserInfoController userController)
        {
            this.timeGetter = timeGetter;
            this.cardStorage = cardStorage;
            StartCoroutine(InitializeStore());
            userInfoController = userController;
        }

        private IEnumerator InitializeStore()
        {
            while (!timeGetter.CanGet())
            {
                yield return null;
            }
            var currentTime = timeGetter.Get();
            var random = new System.Random(currentTime.DayOfYear);
            FillStore(random);
        }

        private void FillStore(System.Random random)
        {
            var cardsByType = new Dictionary<CardRarity, List<int>>();

            foreach(var cardType in cards)
            {
                if (!cardsByType.ContainsKey(cardType))
                {
                    cardsByType[cardType] = cardStorage.FindCardsByType(cardType).ToList();
                }    

                var elligbleCards = cardsByType[cardType];
                var cardIndex = random.Next(0, elligbleCards.Count);
                cardsForPurchase.Add(elligbleCards[cardIndex]);
            }
            isStoreInitialized = true;
        }
        
        public bool CanBuy(int cardId)
        {
            var haveInStore = cardsForPurchase.Contains(cardId);
            var card = cardStorage.IdToValue[cardId];
            var cardOpen = userInfoController.CardIsOpen(card);
            bool canAfford;
            switch (card.ShopCostType)
            {
                case CostType.Gold:
                    canAfford = userInfoController.UserGold >= card.ShopCost;
                    break;
                case CostType.Diamond:
                    canAfford = userInfoController.UserDiamond >= card.ShopCost;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return canAfford && haveInStore && !cardOpen;

        }
        public void Buy(int cardId)
        {
            if (!cardsForPurchase.Contains(cardId))
                Debug.LogWarning("You are buying card that is not present in store!");
            var card = cardStorage.IdToValue[cardId];
            switch (card.ShopCostType)
            {
                case CostType.Gold:
                    userInfoController.UserGold -= card.ShopCost;
                    break;
                case CostType.Diamond:
                    userInfoController.UserDiamond -= card.ShopCost;
                    break;
                default:
                    throw new NotImplementedException();
            }

            userInfoController.UnlockCard(cardId);
        }    
    }
}
