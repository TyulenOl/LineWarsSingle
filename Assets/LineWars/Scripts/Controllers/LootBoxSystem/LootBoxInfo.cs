using LineWars.Model;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.LootBoxes
{
    [CreateAssetMenu(fileName = "New Loot Box Info", menuName = "Loot Boxes/Info")]
    public class LootBoxInfo : ScriptableObject
    {
        [SerializeField] private LootBoxRarity rarity;
        [SerializeField] private List<LootInfo> allLoot;

        public LootBoxRarity Rarity => rarity;
        public IReadOnlyList<LootInfo> AllLoot => allLoot;
    }

    [System.Serializable]
    public class LootInfo
    {
        [SerializeField] private LootType lootType;

        [Header("Gold Settings")]
        [SerializeField] private int minGoldChances;
        [SerializeField] private int maxGoldChances;

        [Header("Diamond Settings")]
        [SerializeField] private int minDiamondChances;
        [SerializeField] private int maxDiamondChances;

        [Header("Upgrade Card Settings")]
        [SerializeField] private int minUpgradeCardChances;
        [SerializeField] private int maxUpgradeCardChances;

        [SerializeField] private List<CardChances> cardChances;

        public LootType LootType => lootType;
        public int MinGoldChances => minGoldChances;
        public int MaxGoldChances => maxGoldChances;
        public int MinDiamondChances => minDiamondChances;
        public int MaxDiamondChances => maxDiamondChances;
        public int MinUpgradeCardChances => minUpgradeCardChances;
        public int MaxUpgradeCardChances => maxUpgradeCardChances; 
        public IReadOnlyList<CardChances> CardChances;
    }

    public class CardChances
    {
        [SerializeField] private CardRarity rarity;
        [SerializeField] private float chance;

        public CardRarity Rarity => rarity;
        public float Chance => chance;
    }
}


