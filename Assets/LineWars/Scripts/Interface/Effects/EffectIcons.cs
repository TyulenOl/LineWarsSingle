using LineWars.Model;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Collections;

namespace LineWars.Interface
{
    [CreateAssetMenu(fileName = "Effect Icons", menuName = "UI/Effect Icons")]
    public class EffectIcons : ScriptableObject,
        IReadOnlyDictionary<EffectType, Sprite>
    {
        [SerializeField] private SerializedDictionary<EffectType, Sprite> effectsIcons;

        public Sprite this[EffectType key] => EffectToIcon[key];

        public IReadOnlyDictionary<EffectType, Sprite> EffectToIcon => effectsIcons;

        public IEnumerable<EffectType> Keys => EffectToIcon.Keys;

        public IEnumerable<Sprite> Values => EffectToIcon.Values;

        public int Count => EffectToIcon.Count;

        public bool ContainsKey(EffectType key)
        {
            return EffectToIcon.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<EffectType, Sprite>> GetEnumerator()
        {
            return EffectToIcon.GetEnumerator();
        }

        public bool TryGetValue(EffectType key, out Sprite value)
        {
            return EffectToIcon.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)EffectToIcon).GetEnumerator();
        }
    }
}
