using UnityEngine;

namespace LineWars.Model
{
    public class UserPurchase
    {
        private string id;
        private string name;
        private string description;
        private Sprite image;
        private int cost;
        private string currencyName;
        private Prize prize;

        public string Id => id;
        public string Name => name;
        public string Description => description;
        public Sprite Image => image;
        public int Cost => cost;
        public string CurrencyName => currencyName;
        public Prize Prize => prize;

        public UserPurchase(string id, string name, string description, Sprite image, int cost, string currencyName, Prize prize)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.cost = cost;
            this.currencyName = currencyName;
            this.prize = prize;
        }
    }
}