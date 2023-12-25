using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DefaultDeck", order = 53)]
    public class DeckPreset : ScriptableObject
    {
        [SerializeField] private string deckName;
        [SerializeField] private List<DeckCard> cards;

        public IEnumerable<DeckCard> Cards => cards;
        public string Name => deckName;
    }
}