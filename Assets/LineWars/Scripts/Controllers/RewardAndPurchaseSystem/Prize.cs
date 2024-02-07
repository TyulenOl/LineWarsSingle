using UnityEngine;

namespace LineWars.Controllers
{
    [System.Serializable]
    public class Prize
    {
        [SerializeField] private PrizeType type;
        [SerializeField] private int amount;

        public PrizeType Type => type;
        public int Amount => amount;

        public Prize(PrizeType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}