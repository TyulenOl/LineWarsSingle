using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars
{
    public interface IStorage<T>
    {
        IReadOnlyDictionary<int, T> IdToValue { get; }
        IReadOnlyDictionary<T, int> ValueToId { get; }
        IEnumerable<T> Values { get; }
        IEnumerable<int> Keys { get; }
        int ValuesCount { get; }
    }

    public abstract class ScriptableStorage<T> :
        ScriptableObject,
        ISerializationCallbackReceiver, 
        IStorage<T>
    {
        [SerializeField] private SerializedDictionary<int, T> idToValue;
        private Dictionary<T, int> valueToId;

        public IReadOnlyDictionary<int, T> IdToValue => idToValue;
        public IReadOnlyDictionary<T, int> ValueToId => valueToId;
        public IEnumerable<int> Keys => idToValue.Keys;
        public IEnumerable<T> Values => idToValue.Values;
        public int ValuesCount => idToValue.Count;


        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            valueToId = new Dictionary<T, int>();
            foreach (var pair in idToValue)
            {
                if (pair.Value == null)
                    return;
                valueToId.TryAdd(pair.Value, pair.Key);
            }
        }
    }
}