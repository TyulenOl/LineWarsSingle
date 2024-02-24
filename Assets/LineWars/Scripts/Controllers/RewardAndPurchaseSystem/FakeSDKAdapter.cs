using System;
using System.Collections.Generic;
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

        public override bool CanBuyProduct(string id)
        {
            return false;
        }

        public override void BuyProduct(string id)
        {
            
        }

        public override ProductData ProductByID(string id)
        {
            return null;
        }

        public override int GetProductCount()
        {
            return 0;
        }

        public override int GetProductCount(PrizeType prizeType)
        {
            return 0;
        }

        public override ProductData[] GetProducts()
        {
            return Array.Empty<ProductData>();
        }

        public override ProductData[] GetProducts(PrizeType prizeType)
        {
            return Array.Empty<ProductData>();
        }

        public override void SendMetrica(string eventName)
        {
            
        }

        public override void SendMetrica(string eventName, IDictionary<string, string> eventParams)
        {
           
        }
    }
}