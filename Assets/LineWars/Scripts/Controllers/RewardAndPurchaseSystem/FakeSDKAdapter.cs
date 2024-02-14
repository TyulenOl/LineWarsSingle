using System;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class FakeSDKAdapter: SDKAdapterBase
    {
        public override bool SDKEnabled => true;

        public override void Initialize()
        {
            GameRoot.Instance.StartGame();
        }
        public override void LockAd()
        {
            
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            Reward(prizeType, amount);
        }

        public override bool CanBuyPurchase(string id)
        {
            return false;
        }

        public override void BuyPurchase(string id)
        {
            
        }

        public override PurchaseData PurchaseByID(string id)
        {
            return null;
        }

        public override int GetPurchaseCount()
        {
            return 0;
        }

        public override int GetPurchaseCount(PrizeType prizeType)
        {
            return 0;
        }

        public override PurchaseData[] GetPurchases()
        {
            return Array.Empty<PurchaseData>();
        }

        public override PurchaseData[] GetPurchases(PrizeType prizeType)
        {
            return Array.Empty<PurchaseData>();
        }
    }
}