using System.Collections.Generic;

namespace LineWars.Model
{
    public class BlessingInitialData
    {
        public IReadOnlyList<(BlessingData, int)> BlessingDataAndCount { get; }
        
        public BlessingInitialData(IReadOnlyList<(BlessingData, int)> blessingDataAndCount)
        {
            BlessingDataAndCount = blessingDataAndCount;
        }
    }
}