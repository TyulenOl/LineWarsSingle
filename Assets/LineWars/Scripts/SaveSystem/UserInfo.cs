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
        public SerializedDictionary<LootBoxType, int> LootBoxes;

        public UserInfo()
        {
            LootBoxes = new();
            foreach (LootBoxType boxType in Enum.GetValues(typeof(LootBoxType)))
            {
                LootBoxes[boxType] = 0;
            }
        }   
    }
}