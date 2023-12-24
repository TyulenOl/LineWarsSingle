using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DeckCard", order = 53)]
    public class DeckCard :
        ScriptableObject,
        IDeckCard
    {
        [SerializeField] private string cardName;
        [SerializeField] private string description;
        [SerializeField] private Unit unit;

        public string Name => cardName;
        public string Description => description;
        public Unit Unit => unit;
    }
}
