using UnityEngine;

namespace LineWars.Model
{
    public interface IReadOnlyCardLevelInfo
    {
        public string CardName { get; }
        public string Description { get; }
        public Sprite Image { get; }
        public Unit Unit { get; }
        public Sprite CardActiveBagLine { get; }
        public Sprite CardInactiveBagLine { get; }
        public int Cost { get; }
        public int CostToUpgrade { get; }
    }
}
