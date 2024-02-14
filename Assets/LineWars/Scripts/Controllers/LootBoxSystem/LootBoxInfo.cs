using System;
using LineWars.Model;
using System.Collections.Generic;
using LineWars.Controllers;
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

        [Header("Count Settings")]
        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;
 

        [Header("Card Settings")]
        [SerializeField] private List<CardChances> cardChances;

        [Header("Blessing Settings")]
        [SerializeField] private List<BlessingChances> blessingChanses;

        public LootType LootType => lootType;
        public int MinCount => minCount;
        public int MaxCount => maxCount;
        public IReadOnlyList<CardChances> CardChances => cardChances;
        public IReadOnlyList<BlessingChances> BlessingChances => blessingChanses;
    }

    [System.Serializable]
    public class CardChances
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private float chance;

        public Rarity Rarity => rarity;
        public float Chance => chance;
    }

    [System.Serializable]
    public class BlessingChances
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private float chance;

        public Rarity Rarity => rarity;
        public float Chance => chance;
    }
}


