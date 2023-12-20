using UnityEngine;

namespace LineWars.Model
{
    public class DeckCard :
        ScriptableObject,
        IDeckCard
    {
        [SerializeField] private string cardName;
        [SerializeField] private string description;
        [SerializeField] private Unit unit;
        [SerializeField] private int cost;

        public string Name => cardName;
        public string Description => description;
        public Unit Unit => unit;
        public int Cost => cost;
    }
}
