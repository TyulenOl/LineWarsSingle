using System;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class FakeSDKAdapter: SDKAdapterBase
    {
        public override bool SDKEnabled => true;
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

        public override UserPurchaseInfo PurchaseByID(string id)
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

        public override UserPurchaseInfo[] GetPurchases()
        {
            return Array.Empty<UserPurchaseInfo>();
        }

        public override UserPurchaseInfo[] GetPurchases(PrizeType prizeType)
        {
            return Array.Empty<UserPurchaseInfo>();
        }
    }
}