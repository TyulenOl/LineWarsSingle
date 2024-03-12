﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DeckCard", order = 53)]
    public class DeckCard : ScriptableObject,
        IDeckCard
    {
        [SerializeField] private Optional<string> overrideCardName;
        [SerializeField] private Optional<string> overrideDescription;
        [SerializeField] private Optional<Sprite> overrideCardImage;
        
        [SerializeField] private Rarity cardRarity;
        [SerializeField] private Unit unit;
        [SerializeField] private int cost;
        [SerializeField] private UnitCost costProgression;
        [SerializeField] private CostType shopCostType;
        [SerializeField] private int shopCost;
        [SerializeField] private List<AdditionalLevelInfo> levels;

        public event Action<DeckCard, int> LevelChanged;

        private FullLevelInfo _zeroLevelInfo;
        private FullLevelInfo ZeroLevelInfo
        {
            get
            {
                if(_zeroLevelInfo == null)
                {
                    _zeroLevelInfo = new FullLevelInfo(
                    overrideCardName.Enabled ? overrideCardName.Value : unit.UnitName,
                    overrideDescription.Enabled ? overrideDescription.Value : unit.UnitDescription,
                    overrideCardImage.Enabled ? overrideCardImage.Value : unit.Sprite,
                    unit,
                    cost,
                    costProgression,
                    0);
                }
                return _zeroLevelInfo;
            }
        }

        private int level;
        public int Level
        {
            get => level;
            set
            {
                var prevLevel = level;
                level = value;
                currentLevelInfo = GetLevelInfo(value);
                if (value == prevLevel)
                    return;
                LevelChanged?.Invoke(this, value);
            }
        }

        private FullLevelInfo currentLevelInfo { get; set; }

        public FullLevelInfo GetLevelInfo(int level)
        {
            if (level == 0)
                return ZeroLevelInfo;
            level--;
            
            if (level < 0 || level >= levels.Count)
                return null;
            
            var additionalLevelInfo = levels[level];
            return ZeroLevelInfo.WithAdditionalInfo(additionalLevelInfo);
        }

        public PurchaseInfo CalculateCost(int unitCount)
        {
            return CostProgression.Calculate(Cost, unitCount);
        }

        public string Name => currentLevelInfo.Name;
        public string Description => currentLevelInfo.Description;
        public Sprite Image => currentLevelInfo.Image;
        public Unit Unit => currentLevelInfo.Unit;
        public Rarity Rarity => cardRarity;
        public int Cost => currentLevelInfo.Cost;
        public UnitCost CostProgression => currentLevelInfo.CostProgression;
        public int ShopCost => shopCost;
        public CostType ShopCostType => shopCostType;
        public int MaxLevel => levels.Count;
    }
}
