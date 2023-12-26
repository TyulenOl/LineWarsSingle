namespace LineWars.Model
{
    public class PurchaseInfo
    {
        public int Cost { get; }
        public bool CanBuy => Cost >= 0;

        public static implicit operator int(PurchaseInfo purchaseInfo)
        {
            return purchaseInfo.Cost;
        }
        
        public PurchaseInfo(int cost)
        {
            Cost = cost;
        }
    }
}