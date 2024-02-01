using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DeckCard", order = 53)]
    public partial class DeckCard : ScriptableObject,
        IDeckCard
    {
        [SerializeField] private Optional<string> cardName;
        [SerializeField] private Optional<string> description;
        [SerializeField] private Optional<Sprite> cardImage;

        [SerializeField] private Sprite cardActiveBagLine;
        [SerializeField] private Sprite cardInactiveBagLine;
        
        [SerializeField] private Rarity cardRarity;
        [SerializeField] private Unit unit;
        [SerializeField] private CostType shopCostType;
        [SerializeField] private int cost; 
        [SerializeField] private int shopCost;
        [SerializeField] private List<AdditionalLevelInfo> levels;

        private FullLevelInfo _zeroLevelInfo;
        private FullLevelInfo ZeroLevelInfo
        {
            get
            {
                if(_zeroLevelInfo == null)
                {
                    _zeroLevelInfo = new FullLevelInfo(
                    cardName.Enabled ? cardName.Value : Unit.UnitName,
                    description.Enabled ? description.Value : Unit.UnitDescription,
                    cardImage.Enabled ? cardImage.Value : Unit.Sprite,
                    unit,
                    cost,
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
                level = value;
                currentLevelInfo = GetLevelInfo(level);
            }
        }

        private FullLevelInfo currentLevelInfo { get; set; }

        public FullLevelInfo GetLevelInfo(int level)
        {
            if (level == 0)
                return ZeroLevelInfo;
            level--;
            var additionalLevelInfo = levels[level];
            return ZeroLevelInfo.WithAdditionalInfo(additionalLevelInfo);
        }


        public string Name => currentLevelInfo.Name;
        public string Description => currentLevelInfo.Description;
        public Sprite Image => currentLevelInfo.Image;
        public Unit Unit => currentLevelInfo.Unit;
        public Rarity Rarity => cardRarity;
        public int Cost => currentLevelInfo.Cost;
        public int ShopCost => shopCost;
        public CostType ShopCostType => shopCostType;
        public Sprite CardActiveBagLine => cardActiveBagLine;
        public Sprite CardInactiveBagLine => cardInactiveBagLine;
        public int MaxLevel => levels.Count;
    }
}
