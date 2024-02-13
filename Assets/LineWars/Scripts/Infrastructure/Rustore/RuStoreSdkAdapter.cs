using System;
using LineWars.Controllers;
using LineWars.Model;
using RuStore.BillingClient;

namespace LineWars
{
    public class RuStoreSdkAdapter: SDKAdapterBase
    {
        public override bool SDKEnabled => RuStoreBillingClient.Instance.IsInitialized;

        public override void Initialize()
        {
            RuStoreBillingClient.Instance.Init();
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanBuyPurchase(string id)
        {
            throw new System.NotImplementedException();
        }

        public override void BuyPurchase(string id)
        {
            throw new System.NotImplementedException();
        }

        public override PurchaseData PurchaseByID(string id)
        {
            throw new System.NotImplementedException();
        }

        public override int GetPurchaseCount()
        {
            throw new System.NotImplementedException();
        }

        public override int GetPurchaseCount(PrizeType prizeType)
        {
            throw new System.NotImplementedException();
        }

        public override PurchaseData[] GetPurchases()
        {
            throw new System.NotImplementedException();
        }

        public override PurchaseData[] GetPurchases(PrizeType prizeType)
        {
            throw new System.NotImplementedException();
        }
    }
}