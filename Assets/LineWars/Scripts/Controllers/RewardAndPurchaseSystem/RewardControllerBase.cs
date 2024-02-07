using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Controllers
{
    public class RewardControllerBase: MonoBehaviour
    {
        [SerializeField] private UnityEvent<PrizeType, int> onAdSuccess;
        public event Action<PrizeType, int> OnAdSuccess;
        
        [SerializeField] private UnityEvent onAdError;
        public event Action OnAdError;
        
        private UserInfoController UserInfoController => GameRoot.Instance.UserController;

        public void RewardForAd(Prize prize) => RewardForAd(prize.Type, prize.Amount);

        public virtual void RewardForAd(PrizeType prizeType, int amount)
        {
            Debug.Log($"RewardForAd {nameof(prizeType)}={prizeType} {nameof(amount)}={amount}");
            Reward(prizeType, amount);
        }
        
        public void Reward(Prize prize)
        {
            Reward(prize.Type, prize.Amount);
        }
        
        public void Reward(PrizeType prizeType, int amount)
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

        protected void SuccessAd(PrizeType prizeType, int amount)
        {
            onAdSuccess?.Invoke(prizeType, amount);
            OnAdSuccess?.Invoke(prizeType, amount);
        }
        
        protected void ErrorAd()
        {
            onAdError?.Invoke();
            OnAdError?.Invoke();
        }
    }
}