using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using UnityEngine;
using YG;
using YG.Utils.Pay;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(ConsumePurchasesYG))]
    public class YandexGameSdkAdapter: SDKAdapterBase
    {
        [SerializeField] private SerializedDictionary<string, Prize> purchaseIdToPrize;
        [SerializeField] private SerializedDictionary<string, Sprite> purchaseIdToSprite;
        
        
        [SerializeField, Min(1)] private int priseTypeLenInBits = 10;
        [SerializeField] private Prize prizeIfAdError;
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent += GetPaymentsEvent;
            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent += OnPurchaseFailedEvent;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent -= GetPaymentsEvent;
            YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent -= OnPurchaseFailedEvent;
        }

        public override bool SdkEnabled => YandexGame.SDKEnabled;

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            if (!SdkEnabled)
            {
                Debug.LogError("Yandex SDK not enabled!");
                return;
            }
            
            try
            {
                var id = RewardUtilities.EncodeId(prizeType, amount, priseTypeLenInBits);
                YandexGame.RewVideoShow(id);
            }
            catch
            {
                Debug.LogError("EncodeId exception!");
                InvokeErrorVideoEvent();
            }
        }

        public override IReadOnlyList<UserPurchase> GetAllPurchases()
        {
            return YandexGame.purchases
                .Select(yg => new UserPurchase
                (
                    yg.id,
                    yg.title,
                    yg.description,
                    purchaseIdToSprite.TryGetValue(yg.id, out var sprite) ? sprite : null,
                    int.TryParse(yg.priceValue, out var value)? value: -1,
                    "Ян",
                    purchaseIdToPrize.TryGetValue(yg.id, out var prize) ? prize : null
                    
                )).ToArray();
        }

        public override bool CanBuyPurchase(string id)
        {
            return purchaseIdToPrize.ContainsKey(id);
        }

        public override void BuyPurchase(string id)
        {
            if (!CanBuyPurchase(id))
            {
                Debug.LogError($"Cant buy this Purchase {nameof(id)}={id}");
                return;
            }
            YandexGame.BuyPayments(id);
        }

        private void OnRewardVideoEvent(int id)
        {
            try
            {
                var (type, amount) = RewardUtilities.DecodeId(id, priseTypeLenInBits);
                Reward(type, amount);
                InvokeRewardVideoEvent(type, amount);
            }
            catch
            {
                Debug.LogError("DecodeId exception!");
                //Reward(prizeIfAdError);
                InvokeErrorVideoEvent();
            }
        }
        
        private void OnErrorVideoEvent()
        {
            InvokeErrorVideoEvent();
        }
        
        private void OnPurchaseSuccessEvent(string id)
        {
            if (purchaseIdToPrize.TryGetValue(id, out var prize))
            {
                Reward(prize);
                InvokePurchaseSuccessEvent(id);
            }
            else
            {
                Debug.LogError("The purchase cannot be processed!");
                InvokeErrorVideoEvent();
            }
        }
        
        private void OnPurchaseFailedEvent(string id)
        {
            InvokePurchaseFailedEvent(id);
        }
        
        private void GetPaymentsEvent()
        {
            InvokePurchasesUpdated();
        }
    }
}