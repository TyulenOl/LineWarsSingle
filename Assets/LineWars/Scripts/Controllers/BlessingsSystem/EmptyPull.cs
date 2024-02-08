using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class EmptyPull: IBlessingsPull
    {
        public IEnumerator<(BlessingId, int)> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[BlessingId id]
        {
            get => 0;
            set {}
        }

        public event Action<BlessingId, int> BlessingCountChanged;
        public bool TryGetCount(BlessingId id, out int count)
        {
            count = 0;
            return false;
        }

        public bool ContainsId(BlessingId id)
        {
            return false;
        }
    }
}