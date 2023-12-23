using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "DeckBuilding/DeckCardStorage", order = 53)]
    public class ScriptableDeckCardsStorage : 
        ScriptableObject,
        IDeckCardsDatabase,
        ISerializationCallbackReceiver
    {
        [SerializeField] private SerializedDictionary<int, DeckCard> idToCard;
        private Dictionary<DeckCard, int> cardToId;

        public IReadOnlyDictionary<int, DeckCard> IdToCard => idToCard;
        public IReadOnlyDictionary<DeckCard, int> CardToId => cardToId;
        public IEnumerable<DeckCard> AllCards => idToCard.Values;
        public int CardsCount => idToCard.Count;


        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            cardToId = new Dictionary<DeckCard, int>();
            foreach (var pair in idToCard)
            {
                if (pair.Value == null)
                    return;
                cardToId.TryAdd(pair.Value, pair.Key);
            }
        }
    }
}
