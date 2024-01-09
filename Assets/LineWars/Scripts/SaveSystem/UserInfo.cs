using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.LootBoxes;

namespace LineWars.Model
{
    [Serializable]
    public class UserInfo
    {
        public int Diamonds;
        public int Gold;
        public List<int> UnlockedCards;
        public int UpgradeCards;
        public SerializedDictionary<LootBoxType, int> LootBoxes = new();
    }
}