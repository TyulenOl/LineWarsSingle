using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Controllers
{
    public abstract class SDKAdapterBase: MonoBehaviour
    {
        [SerializeField] private UnityEvent<PrizeType, int> rewardVideoEvent;
        [SerializeField] private UnityEvent errorVideoEvent;
        [SerializeField] private UnityEvent purchasesUpdatedEvent;
        [SerializeField] private UnityEvent<string> purchaseSuccessEvent;
        [SerializeField] private UnityEvent<string> purchaseFailedEvent;
        [SerializeField] private UnityEvent successLockAd;
        
        public event Action<PrizeType, int> RewardVideoEvent;
        public event Action ErrorVideoEvent;
        public event Action PurchasesUpdated;
        public event Action<string> PurchaseSuccessEvent;
        public event Action<string> PurchaseFailedEvent;
        public event Action SuccesLockAd; 
        
        protected UserInfoController UserInfoController => GameRoot.Instance.UserController;
        public virtual bool NeedInterstitialAd => false;
        
        public abstract void Initialize();
        public abstract bool SDKEnabled { get; }
        public abstract void LockAd();
        public bool AdIsLocked => UserInfoController.LockAd;
        
        public void RewardForAd(Prize prize) => RewardForAd(prize.Type, prize.Amount); 
        protected abstract void RewardForAd(PrizeType prizeType, int amount);
        public abstract bool CanBuyProduct(string id);
        public abstract void BuyProduct(string id);
        public abstract ProductData ProductByID(string id);
        
        public abstract int GetProductCount();
        public abstract int GetProductCount(PrizeType prizeType);
        public abstract ProductData[] GetProducts();
        public abstract ProductData[] GetProducts(PrizeType prizeType);
      
        
        protected void Reward(Prize prize)
        {
            Reward(prize.Type, prize.Amount);
        }

        protected void Reward(PrizeType prizeType, int amount)
        {
            switch (prizeType)
            {
                case PrizeType.Gold:
                    UserInfoController.UserGold += amount;
                    break;
                case PrizeType.Diamonds:
                    UserInfoController.UserDiamond += amount;
                    break;
                case PrizeType.UpgradeCards:
                    UserInfoController.UserUpgradeCards += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(prizeType), prizeType, null);
            }
        }

        protected void InvokeRewardVideoEvent(PrizeType prizeType, int amount)
        {
            rewardVideoEvent?.Invoke(prizeType, amount);
            RewardVideoEvent?.Invoke(prizeType, amount);
        }
        
        protected void InvokeErrorVideoEvent()
        {
            errorVideoEvent?.Invoke();
            ErrorVideoEvent?.Invoke();
        }

        protected void InvokePurchasesUpdated()
        {
            purchasesUpdatedEvent?.Invoke();
            PurchasesUpdated?.Invoke();
        }
        
        protected void InvokePurchaseSuccessEvent(string id)
        {
            purchaseSuccessEvent?.Invoke(id);
            PurchaseSuccessEvent?.Invoke(id);
        }
        
        protected void InvokePurchaseFailedEvent(string id)
        {
            purchaseFailedEvent?.Invoke(id);
            PurchaseFailedEvent?.Invoke(id);
        }

        protected void InvokeSuccessLockAd()
        {
            successLockAd?.Invoke();
            SuccesLockAd?.Invoke();
        }

        public abstract void SendMetrica(string eventName);
        public abstract void SendMetrica(string eventName, IDictionary<string, string> eventParams);

        public void SendCardMetrica(DeckCard deckCard, SceneName sceneName, bool isWin)
        {
            var eventParams = new Dictionary<string, string>()
            {
                {"cardName", deckCard?.Name},
                {"level", sceneName.ToString()},
                {"isWin", isWin.ToString()},
            };
            SendMetrica("card", eventParams);
        }

        public void SendButtonMetrica(string buttonId)
        {
            var eventParams = new Dictionary<string, string>()
            {
                {"buttonId", buttonId},
            };
            
            SendMetrica("button", eventParams);
        }

        protected bool CheckEnableSdk()
        {
            if (!SDKEnabled)
                DebugUtility.LogError("SDK is not enabled!");
            return SDKEnabled;
        }
    }
}