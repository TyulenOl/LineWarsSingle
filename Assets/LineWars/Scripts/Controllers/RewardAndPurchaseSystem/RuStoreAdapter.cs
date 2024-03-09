using UnityEngine;
using LineWars.Model;
using RuStore.BillingClient;
using RuStore;
using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using UnityEngine.SceneManagement;
using YandexMobileAds.Base;

namespace LineWars.Controllers
{
    public class RuStoreAdapter : SDKAdapterBase
    {
        [SerializeField] private string[] productsId;
        [SerializeField] private Sprite diamondsSprite;
        [SerializeField] private float adFreePause;
        [SerializeField] private string interstitialAdUnitId = "demo-interstitial-yandex";
        [SerializeField] private string rewardAdUnitId = "demo-rewarded-yandex";
        private YandexInterstitialAd interstitialAd;
        private YandexRewardAd rewardAd;
        private RuStoreBillingClient ruStoreBillingClient;
        private bool isPurchasesAvailable;
        private List<ProductData> products;
        public override bool SDKEnabled => isPurchasesAvailable;
        private float timeSinceLastAd;

        public override void Initialize()
        {
            interstitialAd = new();
            interstitialAd.Initialize(interstitialAdUnitId);
            rewardAd = new();
            rewardAd.Initialize(rewardAdUnitId);
            ruStoreBillingClient = RuStoreBillingClient.Instance;
            ruStoreBillingClient.Init();
            CheckPurchasesAvailability();
            timeSinceLastAd = adFreePause;
            GameRoot.Instance.StartGame();
            SceneManager.sceneLoaded += OnSceneChanged;
        }

        private void Update()
        {
            timeSinceLastAd += Time.unscaledDeltaTime;
        }

        private void OnSceneChanged(Scene _, LoadSceneMode _1)
        {
            if (timeSinceLastAd >= adFreePause)
            {
                timeSinceLastAd = 0;
                FullScreenAd();
            }
        }

        private void CheckPurchasesAvailability()
        {
            ruStoreBillingClient.CheckPurchasesAvailability(OnError, OnSuccess);

            void OnSuccess(FeatureAvailabilityResult featureAvailabilityResult)
            {
                isPurchasesAvailable = featureAvailabilityResult.isAvailable;
                Debug.Log(featureAvailabilityResult.cause);
                Debug.Log(featureAvailabilityResult.isAvailable);
                Debug.Log(featureAvailabilityResult.cause.name);
                Debug.Log(featureAvailabilityResult.cause.description);
                InitPurchases();
            }

            void OnError(RuStoreError error)
            {
                isPurchasesAvailable = false;
                Debug.Log(error);
                Debug.Log(error.name);
                Debug.Log(error.description);
            }
        }

        private void InitPurchases()
        {
            ruStoreBillingClient.GetProducts(productsId, OnError, OnSuccess);

            void OnError(RuStoreError error)
            {
                products = new List<ProductData>();
            }

            void OnSuccess(List<Product> product)
            {
               foreach (var item in product)
                {
                    products.Add(new ProductData(
                    item.productId,
                    item.title,
                    item.description,
                    diamondsSprite,
                    item.price,
                    item.currency,
                    RewardUtilities.DecodePurchaseId(item.productId)));
                }
            }
        }

        public override void BuyProduct(string id)
        {
            if (!CheckEnableSdk())
                return;

            ruStoreBillingClient.PurchaseProduct(id, 1, null, OnError, OnSuccess);

            void OnError(RuStoreError error)
            {
                OnPurchaseFailedEvent(id);
            }

            void OnSuccess(PaymentResult paymentResult)
            {
                ConfirmPurchase(paymentResult);
            }
        }

        private void OnPurchaseFailedEvent(string id)
        {
            UIPanel.OpenErrorPanel();
            InvokePurchaseFailedEvent(id);
        }

        private void ConfirmPurchase(PaymentResult paymentResult)
        {
            if(paymentResult is PaymentCancelled cancelledPayment)
            {
                OnPurchaseFailedEvent(cancelledPayment.purchaseId);
                return;
            }
            if(paymentResult is PaymentFailure failedPayment)
            {
                OnPurchaseFailedEvent(failedPayment.purchaseId);
                return;
            }
            if(paymentResult is PaymentSuccess successPayment)
            {
                ruStoreBillingClient.ConfirmPurchase(successPayment.purchaseId, OnError, OnSuccess);

                void OnError(RuStoreError error)
                {
                    OnPurchaseFailedEvent(successPayment.purchaseId);
                }

                void OnSuccess()
                {
                    var prize = RewardUtilities.DecodePurchaseId(successPayment.purchaseId);
                    _Reward(prize);
                    UIPanel.OpenSuccessPanel(new Money(prize.Type.ToCostType(), prize.Amount));
                    InvokePurchaseSuccessEvent(successPayment.purchaseId);
                }
            }
        }

        public override bool CanBuyProduct(string id)
        {
            if (!CheckEnableSdk()) return false;
            return RewardUtilities.CanDecodePurchaseId(id);
        }

        private void FullScreenAd()
        {
            DebugUtility.Log("SHOWING AD!");
            interstitialAd.ShowInterstitial();
        }

        public override int GetProductCount()
        {
            if (!CheckEnableSdk())
                return -1;

            return products.Count;
        }

        public override int GetProductCount(PrizeType prizeType)
        {
            if (!CheckEnableSdk())
                return -1;

            return products
                .Where(x => x.Prize != null && x.Prize.Type == prizeType)
                .Count();
        }

        public override ProductData[] GetProducts()
        {
            if (!CheckEnableSdk())
                return null;

            return products.ToArray();
        }

        public override ProductData[] GetProducts(PrizeType prizeType)
        {
            return products
                .Where(x => x.Prize != null && x.Prize.Type == prizeType)
                .ToArray();
        }


        public override void LockAd()
        {
            //НЕ НАДО
        }

        public override ProductData ProductByID(string id)
        {
            return products
                .Where(item => item.Id == id)
                .FirstOrDefault();
        }

        public override void SendMetrica(string eventName)
        {
            //НЕ НАДО
        }

        public override void SendMetrica(string eventName, IDictionary<string, string> eventParams)
        {
            //НЕ НАДО
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        { 
            rewardAd.Rewarded += OnRewarded;
            rewardAd.ShowRewardedAd();

            void OnRewarded(object _, Reward _1)
            {
                rewardAd.Rewarded -= OnRewarded;
                _Reward(prizeType, amount);

                UIPanel.OpenSuccessPanel(new Money(CostType.Gold, amount));
                InvokeRewardVideoEvent(prizeType, amount);
            }
        }
    }
}
