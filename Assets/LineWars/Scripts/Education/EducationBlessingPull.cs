using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;

namespace LineWars.Education
{
    // и локальный и глобольный 
    public class EducationBlessingPull: ILimitingBlessingPool
    {
        private readonly Dictionary<BlessingId, int> blessingsCount;
        public int TotalCount { get; }
        public int CurrentTotalCount { get; private set; }
        
        public event Action<BlessingId, int> BlessingCountChanged;
        public event Action<int, int> CurrentTotalCountChanged;

        public EducationBlessingPull(IDictionary<BlessingId, int> blessingsCount, int totalCount)
        {
            this.blessingsCount = new Dictionary<BlessingId, int>(blessingsCount);
            TotalCount = totalCount;
        }

        public IEnumerator<(BlessingId, int)> GetEnumerator()
        {
            return blessingsCount.Select(pair => (pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[BlessingId id]
        {
            get => blessingsCount[id];
            set
            {
                var prevValue = this[id];
                var diff = value - prevValue;
                if (diff == 0)
                    return;

                CurrentTotalCount += diff;
                
                blessingsCount[id] = value;
                BlessingCountChanged?.Invoke(id, value);
                CurrentTotalCountChanged?.Invoke(CurrentTotalCount, TotalCount);
            }
        }

    
        public bool TryGetCount(BlessingId id, out int count)
        {
            return blessingsCount.TryGetValue(id, out count);
        }

        public bool ContainsId(BlessingId id)
        {
            return blessingsCount.ContainsKey(id);
        }
    }
}