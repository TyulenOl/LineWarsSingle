using System;
using System.Collections.Generic;
using LineWars.Model;

namespace LineWars.Controllers
{
    public interface IBlessingsPull: IEnumerable<(BlessingId, int)>
    {
        public int this[BlessingId id] { get; set; }
        public event Action<BlessingId, int> BlessingCountChanged;
        public bool TryGetCount(BlessingId id, out int count);
        public bool ContainsId(BlessingId id);
    }
}