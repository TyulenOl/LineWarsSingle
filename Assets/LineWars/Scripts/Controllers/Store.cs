using LineWars.Model;
using LineWars.Controllers;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

namespace LineWars.Controllers
{
    public class Store : MonoBehaviour
    {
        [SerializeField, Min(1)] private int changeIntervalInDays = 1;
        [SerializeField] private List<Rarity> cards;
        [SerializeField] private List<DeckCard> exceptions;
        [SerializeField] private SerializedDictionary<BaseBlessing, Money> costDictionary;


        private IGetter<DateTime> timeGetter;
        private IStorage<int, DeckCard> cardStorage;
        private IStorage<BlessingId, BaseBlessing> blessingStorage;
        private UserInfoController userInfoController;

        private bool isStoreInitialized = false;
        private List<int> cardsForPurchase = new();
        private Dictionary<BlessingId, int> blessingToCost;
        private Dictionary<BlessingId, Money> blessingToCostInMoneys;
        private BlessingId[] blessingsForPurchase;

        public bool StoreInitialized => isStoreInitialized;
        public IReadOnlyList<int> CardsForPurchase => cardsForPurchase;
        public IReadOnlyList<BlessingId> BlessingsForPurchase => blessingsForPurchase;
        public IReadOnlyDictionary<BlessingId, int> BlessingToCost => blessingToCost;
        public IReadOnlyDictionary<BlessingId, Money> BlessingToCostInMoneys => blessingToCostInMoneys;
        
        
        public void Initialize(
            IGetter<DateTime> timeGetter, 
            IStorage<int, DeckCard> cardStorage,
            IStorage<BlessingId, BaseBlessing> blessingStorage,
            UserInfoController userController)
        {
            this.timeGetter = timeGetter;
            this.cardStorage = cardStorage;
            this.blessingStorage = blessingStorage;
            userInfoController = userController;
            StartCoroutine(InitializeStore());
            InitializeBlessingCostInfo();
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
        
        private void InitializeBlessingCostInfo()
        {
            blessingToCostInMoneys = costDictionary.ToDictionary(
                x => blessingStorage.ValueToId[x.Key],
                x => x.Value);
            
            blessingToCost = blessingToCostInMoneys.ToDictionary(
                x => x.Key,
                x => x.Value.Amount);
            
            blessingsForPurchase = blessingToCost.Keys.ToArray();
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
/*                    if (userInfoController.CardIsOpen(card))
                        continue;*/

                    cardsForPurchase.Add(card);
                    break;
                }
            }

            isStoreInitialized = true;
        }

        public bool CanBuy(BlessingId blessingId)
        {
            var moneyCost = blessingToCostInMoneys[blessingId]; 
            var canAfford = moneyCost.Type switch
            {
                CostType.Gold => 0 <= moneyCost.Amount && moneyCost.Amount <= userInfoController.UserGold,
                CostType.Diamond => 0 <= moneyCost.Amount && moneyCost.Amount <= userInfoController.UserDiamond,
                _ => throw new NotImplementedException()
            };
            return canAfford;
        }

        public void Buy(BlessingId blessingId)
        {
            var moneyCost = blessingToCostInMoneys[blessingId]; 
            switch (moneyCost.Type)
            {
                case CostType.Gold:
                    userInfoController.UserGold -= moneyCost.Amount;
                    break;
                case CostType.Diamond:
                    userInfoController.UserDiamond -= moneyCost.Amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            userInfoController.GlobalBlessingsPull[blessingId]++;
        }
        
        public bool CanBuy(int cardId)
        {
            var haveInStore = cardsForPurchase.Contains(cardId);
            var card = cardStorage.IdToValue[cardId];
            var cardOpen = userInfoController.CardIsOpen(card);
            bool canAfford = card.ShopCostType switch
            {
                CostType.Gold => userInfoController.UserGold >= card.ShopCost,
                CostType.Diamond => userInfoController.UserDiamond >= card.ShopCost,
                _ => throw new NotImplementedException()
            };

            return canAfford && haveInStore && !cardOpen;

        }

        public bool CanBuy(DeckCard card)
        {
            return CanBuy(cardStorage.ValueToId[card]);
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

        public void Buy(DeckCard card)
        { 
            Buy(cardStorage.ValueToId[card]);
        }
    }
}
