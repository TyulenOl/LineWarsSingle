using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public abstract class ListScriptableStorage<TKey, TValue> : ScriptableObject,
        IStorage<TKey, TValue>
    {
        [SerializeField] private List<TValue> data = new ();

        private Dictionary<TKey, TValue> idToValue;
        private Dictionary<TValue, TKey> valueToId;

        public IReadOnlyDictionary<TKey, TValue> IdToValue => idToValue;
        public IReadOnlyDictionary<TValue, TKey> ValueToId => valueToId;
        public IEnumerable<TValue> Values => idToValue.Values;
        public IEnumerable<TKey> Keys => valueToId.Values;
        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => idToValue;
        public int Count => idToValue.Count;
        
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

        protected abstract TKey GetKey(TValue value);

        public void OnEnable()
        {
            idToValue = new Dictionary<TKey, TValue>(data.Count);
            valueToId = new Dictionary<TValue, TKey>(data.Count);
            
            foreach (var value in data)
            {
                if (value == null)
                {
                    Debug.LogWarning("Null value Exception");
                    continue;
                }
                
                var key = GetKey(value);
                var keyContains = idToValue.ContainsKey(key);
                var valueContains = valueToId.ContainsKey(value);
                
                if (!keyContains && !valueContains)
                {
                    idToValue[key] = value;
                    valueToId[value] = key;
                }
                else
                {
                    Debug.LogWarning($"The storage contains equals members! {key} {value}");
                }
            }
        }
    }
}