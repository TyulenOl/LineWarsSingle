using System;
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
        
        public event Action<PrizeType, int> RewardVideoEvent;
        public event Action ErrorVideoEvent;
        public event Action PurchasesUpdated;
        public event Action<string> PurchaseSuccessEvent;
        public event Action<string> PurchaseFailedEvent;
        
        protected UserInfoController UserInfoController => GameRoot.Instance.UserController;
        
        public abstract void Initialize();
        public abstract bool SDKEnabled { get; }
        public void RewardForAd(Prize prize) => RewardForAd(prize.Type, prize.Amount); 
        protected abstract void RewardForAd(PrizeType prizeType, int amount);
        public abstract bool CanBuyPurchase(string id);
        public abstract void BuyPurchase(string id);
        public abstract PurchaseData PurchaseByID(string id);
        
        public abstract int GetPurchaseCount();
        public abstract int GetPurchaseCount(PrizeType prizeType);
        public abstract PurchaseData[] GetPurchases();
        public abstract PurchaseData[] GetPurchases(PrizeType prizeType);

        
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
    }
}