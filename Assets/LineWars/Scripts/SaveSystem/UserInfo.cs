using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.LootBoxes;

namespace LineWars.Model
{
    [Serializable]
    public class UserInfo : IReadOnlyUserInfo
    {
        public int Diamonds;
        public int Gold;
        public List<int> UnlockedCards;
        public int UpgradeCards;
        public SerializedDictionary<LootBoxType, int> LootBoxes;
        public int PassingGameModes;
        
        int IReadOnlyUserInfo.Diamonds => Diamonds;
        int IReadOnlyUserInfo.Gold => Gold;
        IReadOnlyList<int> IReadOnlyUserInfo.UnlockedCards => UnlockedCards;
        int IReadOnlyUserInfo.UpgradeCards => UpgradeCards;
        IReadOnlyDictionary<LootBoxType, int> IReadOnlyUserInfo.LootBoxes => LootBoxes;
    }

    public interface IReadOnlyUserInfo
    {
        public int Diamonds { get; }
        public int Gold { get; }
        public IReadOnlyList<int> UnlockedCards { get; }
        public int UpgradeCards { get; }
        public IReadOnlyDictionary<LootBoxType, int> LootBoxes { get; }
    }
}