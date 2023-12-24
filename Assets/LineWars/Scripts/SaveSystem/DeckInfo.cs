using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    [Serializable]
    public class DeckInfo
    {
        public int Id;
        public string Name;
        public DeckCardInfo[] Cards;
    }
}
