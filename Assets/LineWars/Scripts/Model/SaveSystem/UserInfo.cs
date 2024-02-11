using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Model;

namespace LineWars.Model
{
    [Serializable]
    public class UserInfo : IReadOnlyUserInfo
    {
        public int Diamonds;
        public int Gold;
        public int UpgradeCards;
        public int PassingGameModes;
        public bool DefaultBlessingsIsAdded;
        
        public SerializedDictionary<LootBoxType, int> LootBoxes;
        public SerializedDictionary<int, int> CardLevels;
        public SerializedDictionary<BlessingId, int> Blessings;
        
        public List<int> UnlockedCards;
        public List<BlessingId> SelectedBlessings;
        public List<string> UsedPromoCodes;
        
        int IReadOnlyUserInfo.Diamonds => Diamonds;
        int IReadOnlyUserInfo.Gold => Gold;
        IReadOnlyList<int> IReadOnlyUserInfo.UnlockedCards => UnlockedCards;
        int IReadOnlyUserInfo.UpgradeCards => UpgradeCards;
        IReadOnlyDictionary<LootBoxType, int> IReadOnlyUserInfo.LootBoxes => LootBoxes;
        IReadOnlyDictionary<int, int> IReadOnlyUserInfo.CardLevels => CardLevels;

        bool IReadOnlyUserInfo.DefaultBlessingsIsAdded => DefaultBlessingsIsAdded;
        IReadOnlyDictionary<BlessingId, int> IReadOnlyUserInfo.Blessings => Blessings;
    }

    public interface IReadOnlyUserInfo
    {
        public int Diamonds { get; }
        public int Gold { get; }
        public IReadOnlyList<int> UnlockedCards { get; }
        public int UpgradeCards { get; }
        public IReadOnlyDictionary<LootBoxType, int> LootBoxes { get; }
        public IReadOnlyDictionary<int, int> CardLevels { get; }
        
        public bool DefaultBlessingsIsAdded { get; }
        public IReadOnlyDictionary<BlessingId, int> Blessings { get; }
    }
}