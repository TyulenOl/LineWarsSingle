using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Loot Box Info", menuName = "Loot Boxes/Info")]
    public class LootBoxInfo : ScriptableObject
    {
        [SerializeField] private string boxName;
        [SerializeField] private string boxDescription;
        [SerializeField] private Sprite boxSprite;
        [SerializeField] private GameObject bg;
        [SerializeField] private LootBoxType boxType;
        [SerializeField] private List<LootInfo> allLoot;
        [Header("Cost Settings")]
        [SerializeField] private CostType costType;
        [SerializeField] private int cost;

        public string Name => boxName;
        public Sprite BoxSprite => boxSprite;
        public GameObject Bg => bg;
        public CostType CostType => costType;
        public int Cost => cost;
        public LootBoxType BoxType => boxType;
        public IReadOnlyList<LootInfo> AllLoot => allLoot;

        public string Description => boxDescription;
        
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
        [SerializeField] private List<BlessingChances> blessingChances;

        public LootType LootType => lootType;
        public int MinGoldChances => minGoldChances;
        public int MaxGoldChances => maxGoldChances;
        public int MinDiamondChances => minDiamondChances;
        public int MaxDiamondChances => maxDiamondChances;
        public int MinUpgradeCardChances => minUpgradeCardChances;
        public int MaxUpgradeCardChances => maxUpgradeCardChances;
        public IReadOnlyList<CardChances> CardChances => cardChances;

        public IReadOnlyList<BlessingChances> BlessingChances => blessingChances;
    }

    [Serializable]
    public class CardChances
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private float chance;

        public Rarity Rarity => rarity;
        public float Chance => chance;
    }

    [Serializable]
    public class BlessingChances
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private float chance;

        public Rarity Rarity => rarity;
        public float Chance => chance;
    }
}


