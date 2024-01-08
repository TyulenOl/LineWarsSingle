using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DeckCard", order = 53)]
    public class DeckCard : ScriptableObject,
        IDeckCard
    {
        [SerializeField] private Optional<string> cardName;
        [SerializeField] private Optional<string> description;
        [SerializeField] private Optional<Sprite> cardImage;
        [SerializeField] private CardRarity cardRarity;
        [SerializeField] private Unit unit;
        [SerializeField] private int cost; 

        
        public string Name => cardName.Enabled ? cardName.Value : Unit.UnitName;
        public string Description => description.Enabled ? description.Value : Unit.UnitDescription;
        public Sprite Image => cardImage.Enabled ? cardImage.Value : Unit.Sprite;
        public Unit Unit => unit;
        public CardRarity Rarity => cardRarity;
        public int Cost => cost;
    }
}
