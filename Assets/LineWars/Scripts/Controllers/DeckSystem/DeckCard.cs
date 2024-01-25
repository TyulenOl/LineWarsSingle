using LineWars.LootBoxes;
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
        
        [SerializeField] private CardRarity cardRarity;
        [SerializeField] private Unit unit;
        [SerializeField] private CostType shopCostType;
        [SerializeField] private int cost; 
        [SerializeField] private int shopCost;
        [SerializeField] private List<CardLevelInfo> levels;

        public IReadOnlyList<IReadOnlyCardLevelInfo> Levels => levels.Cast<IReadOnlyCardLevelInfo>().ToList();
        
        public string Name => cardName.Enabled ? cardName.Value : Unit.UnitName;
        public string Description => description.Enabled ? description.Value : Unit.UnitDescription;
        public Sprite Image => cardImage.Enabled ? cardImage.Value : Unit.Sprite;
        public Unit Unit => unit;
        public CardRarity Rarity => cardRarity;
        public int Cost => cost;
        public int ShopCost => shopCost;
        public CostType ShopCostType => shopCostType;
        public Sprite CardActiveBagLine => cardActiveBagLine;
        public Sprite CardInactiveBagLine => cardInactiveBagLine;
        public int MaxLevel => levels.Count;

        public IReadOnlyCardLevelInfo GetLevel(int level)
        {
            return levels[level + 1];
        }
    }
}
