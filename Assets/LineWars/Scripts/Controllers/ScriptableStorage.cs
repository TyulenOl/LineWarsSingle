using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars
{
    public interface IStorage<TKey,TValue>
    {
        IReadOnlyDictionary<TKey, TValue> IdToValue { get; }
        IReadOnlyDictionary<TValue, TKey> ValueToId { get; }
        IEnumerable<TValue> Values { get; }
        IEnumerable<TKey> Keys { get; }
        int ValuesCount { get; }
    }

    public abstract class ScriptableStorage<TKey, TValue> :
        ScriptableObject,
        ISerializationCallbackReceiver, 
        IStorage<TKey,TValue>
    {
        [SerializeField] private SerializedDictionary<TKey, TValue> idToValue;
        private Dictionary<TValue, TKey> valueToId;

        public IReadOnlyDictionary<TKey, TValue> IdToValue => idToValue;
        public IReadOnlyDictionary<TValue, TKey> ValueToId => valueToId;
        public IEnumerable<TKey> Keys => idToValue.Keys;
        public IEnumerable<TValue> Values => idToValue.Values;
        public int ValuesCount => idToValue.Count;


        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            valueToId = new Dictionary<TValue, TKey>();
            foreach (var pair in idToValue)
            {
                if (pair.Value == null)
                    return;
                valueToId.TryAdd(pair.Value, pair.Key);
            }
        }
    }
}