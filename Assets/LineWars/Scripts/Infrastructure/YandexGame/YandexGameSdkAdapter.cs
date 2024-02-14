using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using YG;
using YG.Utils.Pay;

namespace LineWars.Controllers
{
    public class YandexGameSdkAdapter: SDKAdapterBase
    {
        private static bool consume;
        private static bool dataInitialized;
        
        [Space]
        [SerializeField, Min(1)] private int priseTypeLenInBits = 10;
        [Space]
        [SerializeField] private SerializedDictionary<string, Prize> purchaseIdToPrize;
        [Tooltip("Соотношение сторон 1:1")]
        [SerializeField] private SerializedDictionary<string, Sprite> purchaseIdToSprite;
        [SerializeField] private SerializedDictionary<string, Prize> promoCodes = new();
        [SerializeField] private string lockAdId;
        
        
        [Header("References")]
        [SerializeField] private YandexGameProvider yandexGameProvider;
        
        public override bool SDKEnabled => YandexGame.SDKEnabled;
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent += OnPaymentsEvent;
            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent += OnPurchaseFailedEvent;
            
            YandexGame.GetDataEvent += InitializeData;
            YandexGame.GetDataEvent += ConsumePurchases;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent -= OnPaymentsEvent;
            YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent -= OnPurchaseFailedEvent;
            
            YandexGame.GetDataEvent -= InitializeData;
            YandexGame.GetDataEvent -= ConsumePurchases;
        }

        public override void Initialize()
        {
            if (SDKEnabled)
            {
                ConsumePurchases();
                InitializeData();
            }
        }
        
        private void ConsumePurchases()
        {
            if (consume) return;
            consume = true;
            
            YandexGame.ConsumePurchases();
        }
        
        private void InitializeData()
        {
            if (dataInitialized) return;
            dataInitialized = true;
            
            yandexGameProvider.LoadAll();
            GameRoot.Instance.StartGame();
            
            UsePromoCode();
        }

        private void UsePromoCode()
        {
            var promoCode = YandexGame.EnvironmentData.payload;
            if (!string.IsNullOrEmpty(promoCode)
                && promoCodes.TryGetValue(promoCode, out var prize)
                && !UserInfoController.PromoCodeIsUsed(promoCode))
            {
                Reward(prize);
                UserInfoController.UsePromoCode(promoCode);
                FullscreenPanel.OpenPromoCodePanel(promoCode);
            }
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            if (!CheckEnableSdk()) return;
            
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

        public override PurchaseData PurchaseByID(string id)
        {
            if (!CheckEnableSdk()) return null;
            
            return ConvertPurchase(YandexGame.PurchaseByID(id));
        }

        public override PurchaseData[] GetPurchases() 
        {
            if (!CheckEnableSdk()) return null;
            
            return YandexGame.purchases
                .Select(ConvertPurchase)
                .ToArray();
        }

        public override PurchaseData[] GetPurchases(PrizeType prizeType)
        {
            if (!CheckEnableSdk()) return null;
            
            return GetPurchases()
                .Where(x => x.Prize != null && x.Prize.Type == prizeType)
                .ToArray();
        }

        public override void LockAd()
        {
            BuyPurchase(lockAdId);
        }

        private void _LockAd()
        {
            //YandexGame.Instance.infoYG.AdWhenLoadingScene = false;
            //TODO
        }

        public override int GetPurchaseCount()
        {
            if (!CheckEnableSdk()) return -1;
            return YandexGame.purchases.Length;
        }

        public override int GetPurchaseCount(PrizeType prizeType)
        {
            if (!CheckEnableSdk()) return -1;
            return GetPurchases(prizeType).Length;
        }
        
        public override bool CanBuyPurchase(string id)
        {
            if (!CheckEnableSdk()) return false;
            return purchaseIdToPrize.ContainsKey(id);
        }

        public override void BuyPurchase(string id)
        {
            if (!CheckEnableSdk()) return;
            
            if (!CanBuyPurchase(id))
            {
                Debug.LogError($"Cant buy this Purchase {nameof(id)}={id}");
                return;
            }
            YandexGame.BuyPayments(id);
        }
        
        private PurchaseData ConvertPurchase(Purchase yg)
        {
            return new PurchaseData(
                yg.id,
                yg.title,
                yg.description,
                purchaseIdToSprite.TryGetValue(yg.id, out var sprite) ? sprite : null, 
                int.TryParse(yg.priceValue, out var value) ? value : -1, 
                "Ян",
                purchaseIdToPrize.TryGetValue(yg.id, out var prize) ? prize : null);
        }

        private void OnRewardVideoEvent(int id)
        {
            Debug.Log("OnRewardVideoEvent");
            try
            {
                var (type, amount) = RewardUtilities.DecodeId(id, priseTypeLenInBits);
                Reward(type, amount);
                
                FullscreenPanel.OpenSuccessPanel(new Money(CostType.Gold, amount));
                InvokeRewardVideoEvent(type, amount);
            }
            catch
            {
                Debug.LogError("DecodeId exception!");
                
                FullscreenPanel.OpenErrorPanel();
                InvokeErrorVideoEvent();
            }
        }
        
        private void OnErrorVideoEvent()
        {
            FullscreenPanel.OpenErrorPanel();
            InvokeErrorVideoEvent();
        }
        
        private void OnPurchaseSuccessEvent(string id)
        {
            if (id == lockAdId)
            {
                UserInfoController.LockAd = true;
                FullscreenPanel.OpenSuccessLockAdPanel();
                InvokeSuccessLockAd();
                InvokePurchaseSuccessEvent(id);
                return;
            }
            
            if (purchaseIdToPrize.TryGetValue(id, out var prize))
            {
                Reward(prize);
                FullscreenPanel.OpenSuccessPanel(new Money(prize.Type.ToCostType(), prize.Amount));
                InvokePurchaseSuccessEvent(id);
            }
            else
            {
                Debug.LogError("The purchase cannot be processed!");
                FullscreenPanel.OpenErrorPanel();
                InvokeErrorVideoEvent();
            }
        }
        
        private void OnPurchaseFailedEvent(string id)
        {
            FullscreenPanel.OpenErrorPanel();
            InvokePurchaseFailedEvent(id);
        }
        
        private void OnPaymentsEvent()
        {
            InvokePurchasesUpdated();
        }
        
        private bool CheckEnableSdk()
        {
            if (!SDKEnabled)
                DebugUtility.LogError("Yandex SDK not enabled!");
            return SDKEnabled;
        }
    }
}