using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

namespace LineWars.Model
{
    public class AllCards : 
        ScriptableObject,
        IAllCards,
        ISerializationCallbackReceiver
    {
        [SerializeField] private SerializedDictionary<int, DeckCard> idToCard;
        private Dictionary<DeckCard, int> cardToId;

        public IReadOnlyDictionary<int, DeckCard> IdToCard => idToCard;
        public IReadOnlyDictionary<DeckCard, int> CardToId => cardToId;

        public void OnAfterDeserialize()
        {
            cardToId = idToCard.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
