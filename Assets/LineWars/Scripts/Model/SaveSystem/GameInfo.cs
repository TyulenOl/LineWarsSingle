using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    [Serializable]
    public class GameInfo
    {
        public Dictionary<int, DeckInfo> Decks;
        public Dictionary<int, MissionInfo> Missions;
        public UserInfo UserInfo;
    }
}