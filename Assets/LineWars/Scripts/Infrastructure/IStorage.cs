using System.Collections.Generic;

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
}