using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.LootBoxes;
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
        [SerializeField] private SerializedDictionary<LootBoxType, int> defaultBoxesCount;
        [SerializeField] private SerializedDictionary<BlessingId, int> defaultBlessingsCount;
        
        
        public int DefaultGold => defaultGold;
        public int DefaultDiamond => defaultDiamond;
        public IEnumerable<DeckCard> DefaultCards => defaultCards;
        public IReadOnlyDictionary<LootBoxType, int> DefaultBoxesCount => defaultBoxesCount;
        public IReadOnlyDictionary<BlessingId, int> DefaultBlessingsCount => defaultBlessingsCount;
    }
}