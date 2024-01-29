using LineWars.Model;
using LineWars.Controllers;
using LineWars.LootBoxes;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

namespace LineWars.Store
{
    public class CardStore : MonoBehaviour
    {
        [SerializeField, Min(1)] private int changeIntervalInDays = 1;
        [SerializeField] private List<Rarity> cards;
        [SerializeField] private List<DeckCard> exceptions;
        [SerializeField] private SerializedDictionary<BlessingId, int> blessingsCost;

        private IGetter<DateTime> timeGetter;
        private IStorage<int, DeckCard> cardStorage;
        private UserInfoController userInfoController;

        private bool isStoreInitialized = false;
        private List<int> cardsForPurchase = new();
        private List<BlessingId> blessingsForPurchase;

        public bool StoreInitialized => isStoreInitialized;
        public IReadOnlyList<int> CardsForPurchase => cardsForPurchase;
        public IReadOnlyList<BlessingId> BlessingsForPurchase => blessingsForPurchase;
        public IReadOnlyDictionary<BlessingId, int> BlessingsCost => blessingsCost;

        public void Initialize(
            IGetter<DateTime> timeGetter, 
            IStorage<int, DeckCard> cardStorage,
            UserInfoController userController)
        {
            this.timeGetter = timeGetter;
            this.cardStorage = cardStorage;
            StartCoroutine(InitializeStore());
            userInfoController = userController;
            blessingsForPurchase = BlessingsCost.Keys.ToList();
        }

        private IEnumerator InitializeStore()
        {
            while (!timeGetter.CanGet())
            {
                yield return null;
            }
            var currentTime = timeGetter.Get();
            var random = new System.Random(currentTime.DayOfYear / changeIntervalInDays);
            FillStore(random);
        }

        private void FillStore(System.Random random)
        {
            var cardsByType = new Dictionary<Rarity, List<int>>();

            foreach(var cardType in cards)
            {
                if (!cardsByType.ContainsKey(cardType))
                {
                    cardsByType[cardType] = cardStorage.FindCardsByType(cardType).ToList();
                }    

                var elligbleCards = cardsByType[cardType];
                if(elligbleCards.Count == 0)
                {
                    Debug.LogWarning($"Store couldn't find DeckCard of rarity: {cardType}");
                    continue;
                }
                while(true)
                {
                    var cardIndex = random.Next(0, elligbleCards.Count);
                    var card = elligbleCards[cardIndex];
                    if(cardsForPurchase.Contains(card))
                        continue;
                    if (exceptions.Contains(cardStorage.IdToValue[card]))
                        continue;
                    cardsForPurchase.Add(card);
                    break;
                }
            }

            isStoreInitialized = true;
        }

        public bool CanBuy(BlessingId blessingId)
        {
            return false;
        }

        public void Buy(BlessingId blessingId)
        {
            
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
