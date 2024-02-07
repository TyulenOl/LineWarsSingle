using System;
using UnityEngine;
using YG;

namespace LineWars.Controllers
{
    public class YandexGameRewardController: RewardControllerBase
    {
        [SerializeField, Min(1)] private int priseTypeLenInBits = 10;
        [SerializeField] private Prize prizeIfAdError;
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += RewardVideoEvent;
            YandexGame.ErrorVideoEvent += ErrorVideoEvent;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= RewardVideoEvent;
            YandexGame.ErrorVideoEvent -= ErrorVideoEvent;
        }


        public override void RewardForAd(PrizeType prizeType, int amount)
        {
            try
            {
                var id = RewardUtilities.EncodeId(prizeType, amount, priseTypeLenInBits);
                YandexGame.RewVideoShow(id);
            }
            catch
            {
                Debug.LogError("EncodeId exception!");
                ErrorAd();
            }
        }
        
        private void RewardVideoEvent(int id)
        {
            try
            {
                var (type, amount) = RewardUtilities.DecodeId(id, priseTypeLenInBits);
                Reward(type, amount);
                SuccessAd(type, amount);
            }
            catch
            {
                Debug.LogError("DecodeId exception!");
                Reward(prizeIfAdError);
                ErrorAd();
            }
        }
        
        private void ErrorVideoEvent()
        {
            ErrorAd();
        }
    }
}