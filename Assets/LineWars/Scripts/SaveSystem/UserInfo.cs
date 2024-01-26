using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.LootBoxes;

namespace LineWars.Model
{
    [Serializable]
    public class UserInfo : IReadOnlyUserInfo, IEquatable<UserInfo>
    {
        public int Diamonds;
        public int Gold;
        public List<int> UnlockedCards;
        public int UpgradeCards;
        public SerializedDictionary<LootBoxType, int> LootBoxes;
        public int PassingGameModes;

        public bool DefaultBlessingsIsAdded;
        public SerializedDictionary<BlessingId, int> Blessings;
        private bool defaultBlessingsIsAdded;

        int IReadOnlyUserInfo.Diamonds => Diamonds;
        int IReadOnlyUserInfo.Gold => Gold;
        IReadOnlyList<int> IReadOnlyUserInfo.UnlockedCards => UnlockedCards;
        int IReadOnlyUserInfo.UpgradeCards => UpgradeCards;
        IReadOnlyDictionary<LootBoxType, int> IReadOnlyUserInfo.LootBoxes => LootBoxes;

        bool IReadOnlyUserInfo.DefaultBlessingsIsAdded => defaultBlessingsIsAdded;
        IReadOnlyDictionary<BlessingId, int> IReadOnlyUserInfo.Blessings => Blessings;

        public bool Equals(UserInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            
            return Diamonds == other.Diamonds
                   && Gold == other.Gold 
                   && (UnlockedCards.Count == other.UnlockedCards.Count && !UnlockedCards.Except(other.UnlockedCards).Any())
                   && UpgradeCards == other.UpgradeCards 
                   && (LootBoxes.Count == other.LootBoxes.Count && !LootBoxes.Except(other.LootBoxes).Any())
                   && PassingGameModes == other.PassingGameModes
                   && (Blessings.Count == other.Blessings.Count && !Blessings.Except(other.Blessings).Any());
        }
    }

    public interface IReadOnlyUserInfo
    {
        public int Diamonds { get; }
        public int Gold { get; }
        public IReadOnlyList<int> UnlockedCards { get; }
        public int UpgradeCards { get; }
        public IReadOnlyDictionary<LootBoxType, int> LootBoxes { get; }
        
        public bool DefaultBlessingsIsAdded { get; }
        public IReadOnlyDictionary<BlessingId, int> Blessings { get; }
    }
}