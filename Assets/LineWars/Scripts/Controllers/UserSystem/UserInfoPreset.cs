using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "DeckBuilding/UserInfoPreset", order = 53)]
    public class UserInfoPreset: ScriptableObject
    {
        [SerializeField] private int defaultGold;
        [SerializeField] private int defaultDiamond;
        [SerializeField] private List<DeckCard> defaultCards;
        [SerializeField] private SerializedDictionary<int, int> defaultCardLevels;
        [SerializeField] private SerializedDictionary<LootBoxType, int> defaultBoxesCount;
        [SerializeField] private SerializedDictionary<BaseBlessing, int> defaultBlessingsCount;
        [SerializeField] private List<BaseBlessing> defaultSelectedBlessings;
        
        
        public int DefaultGold => defaultGold;
        public int DefaultDiamond => defaultDiamond;
        public IEnumerable<DeckCard> DefaultCards => defaultCards;
        public IReadOnlyDictionary<LootBoxType, int> DefaultBoxesCount => defaultBoxesCount;
        public IReadOnlyDictionary<int, int> DefaultCardLevels => defaultCardLevels;
        public IReadOnlyDictionary<BaseBlessing, int> DefaultBlessingsCount => defaultBlessingsCount;
        public IReadOnlyList<BaseBlessing> DefaultSelectedBlessings => defaultSelectedBlessings;

        private void OnValidate()
        {
            foreach(var cardInfo in defaultCardLevels)
            {
                var level = cardInfo.Value;
                var cardId = cardInfo.Key;
                if (level < 1)
                {
                    Debug.LogError($"Level Can't be less than zero! Card Id: {cardId}");   
                }
            }
        }
    }
}