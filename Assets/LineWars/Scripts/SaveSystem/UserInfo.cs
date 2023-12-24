using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    [Serializable]
    public class UserInfo
    {
        public int amountInGameCurrency;
        public List<int> unlockCards;
    }
}