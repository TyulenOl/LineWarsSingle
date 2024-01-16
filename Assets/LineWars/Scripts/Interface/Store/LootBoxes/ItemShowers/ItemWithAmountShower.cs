using TMPro;
using UnityEngine;

namespace LineWars
{
    public class ItemWithAmountShower : LootedItemShower
    {
        [SerializeField] private TMP_Text amount;

        public void ShowItem(int amount)
        {
            this.amount.text = amount.ToString();
        }
    }
}