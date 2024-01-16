using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

public static class EnumerableExtensions
{
    public static SerializedDictionary<TKey, TValue> ToSerializedDictionary<TSource, TKey, TValue>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TValue> valueSelector)
    {
        return new SerializedDictionary<TKey, TValue>(
            source.Select(el => 
                    new KeyValuePair<TKey, TValue>(
                        keySelector(el),
                        valueSelector(el))
            ));
    }
}
