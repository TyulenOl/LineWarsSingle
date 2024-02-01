using UnityEngine;

namespace LineWars.Model
{
    public partial class DeckCard
    {
        [System.Serializable]
        public class CardLevelInfo : IReadOnlyCardLevelInfo
        {
            public DeckCard Card { get; set; }
            [SerializeField] private Optional<string> cardName;
            [SerializeField] private Optional<string> description;
            [SerializeField] private Optional<Sprite> cardImage;
            [SerializeField] private Optional<Unit> unit;
            [SerializeField] private Optional<Sprite> cardActiveBagLine;
            [SerializeField] private Optional<Sprite> cardInactiveBagLine;
            [SerializeField] private Optional<int> cost;
            [SerializeField] private int costToUpgrade;

            public string CardName => cardName.Enabled ? cardName.Value : Card.Name;
            public string Description => description.Enabled ? cardName.Value : Card.Description;
            public Sprite Image => cardImage.Enabled ? cardImage.Value : Card.Image;
            public Unit Unit => Card.unit;
            public Sprite CardActiveBagLine => cardActiveBagLine.Enabled ? cardActiveBagLine.Value : Card.cardActiveBagLine;
            public Sprite CardInactiveBagLine => cardInactiveBagLine.Enabled ? cardInactiveBagLine.Value : Card.cardInactiveBagLine;
            public int Cost => cost.Enabled ? cost.Value : Card.cost;
            public int CostToUpgrade => costToUpgrade;
        }
    }
}
