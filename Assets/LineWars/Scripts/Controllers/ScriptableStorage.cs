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
        IEnumerable<KeyValuePair<TKey, TValue>> Pairs { get; }
        int Count { get; }
        bool TryGetKey(TValue value, out TKey key);
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
        bool ContainsValue(TValue key);
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
        public int Count => idToValue.Count;
        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => idToValue;
        public bool TryGetKey(TValue value, out TKey key)
        {
            return valueToId.TryGetValue(value, out key);
        }

        public bool ContainsKey(TKey key)
        {
            return idToValue.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return idToValue.TryGetValue(key, out value);
        }

        public bool ContainsValue(TValue key)
        {
            return valueToId.ContainsKey(key);
        }


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