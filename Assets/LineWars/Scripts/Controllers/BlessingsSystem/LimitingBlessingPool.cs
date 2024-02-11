using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class LimitingBlessingPool: ILimitingBlessingPool
    {
        private readonly IBlessingsPull innerPull;
        private readonly HashSet<BlessingId> blessings;
        public int CurrentTotalCount { get; private set; }
        public int TotalCount { get; }
        
        public event Action<int, int> CurrentTotalCountChanged;
        public event Action<BlessingId, int> BlessingCountChanged;

        public LimitingBlessingPool(
            IBlessingsPull innerPull,
            IEnumerable<BlessingId> blessings,
            int totalCount)
        {
            if (this.CurrentTotalCount < 0)
                throw new ArgumentException(nameof(totalCount));
            
            this.innerPull = innerPull;
            CurrentTotalCount = totalCount;
            TotalCount = totalCount;
            this.blessings = blessings.ToHashSet();
        }
        
        public IEnumerator<(BlessingId, int)> GetEnumerator()
        {
            foreach (var id in blessings)
            {
                if (innerPull.TryGetCount(id, out var innerCount))
                {
                    yield return (id, Math.Min(innerCount, CurrentTotalCount));
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[BlessingId id]
        {
            get => TryGetCount(id, out var count) ? count : throw new KeyNotFoundException();
            set
            {
                if (!blessings.Contains(id))
                    throw new ArgumentException("Cant add new blessings after initialize");
                if (value < 0)
                    throw new ArgumentException("BlessingsCount can't be less than zero!");
                var prevValue = this[id];
                var diff = value - prevValue;
                if (diff == 0)
                    return;
                innerPull[id] += diff;
                CurrentTotalCount += diff;

                foreach (var (blessing, count) in this)
                    BlessingCountChanged?.Invoke(blessing, count);
                CurrentTotalCountChanged?.Invoke(CurrentTotalCount, TotalCount);
            }
        }
        
        public bool TryGetCount(BlessingId id, out int count)
        {
            count = 0;
            if (innerPull.TryGetCount(id, out var innerCount) 
                && blessings.Contains(id))
            {
                count = Math.Min(innerCount, CurrentTotalCount);
                return true;
            }

            return false;
        }

        public bool ContainsId(BlessingId id)
        {
            return TryGetCount(id, out _);
        }
    }
}