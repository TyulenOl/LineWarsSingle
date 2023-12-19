using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    [Serializable]
    public class DeckInfo
    {
        public string Name;
        public List<DeckCardInfo> Cards = new();
    }
}
