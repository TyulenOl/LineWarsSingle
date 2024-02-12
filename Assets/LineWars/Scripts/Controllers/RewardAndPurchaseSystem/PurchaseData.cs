using UnityEngine;

namespace LineWars.Model
{
    public class PurchaseData
    {
        private string id;
        private string title;
        private string description;
        private Sprite sprite;
        private int priceValue;
        private string currencyName;
        private Prize prize;

        public string Id => id;
        public string Title => title;
        public string Description => description;
        public Sprite Sprite => sprite;
        public int PriceValue => priceValue;
        public string CurrencyName => currencyName;
        public Prize Prize => prize;

        public PurchaseData(string id, string title, string description, Sprite sprite, int priceValue, string currencyName, Prize prize)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.sprite = sprite;
            this.priceValue = priceValue;
            this.currencyName = currencyName;
            this.prize = prize;
        }
    }
}