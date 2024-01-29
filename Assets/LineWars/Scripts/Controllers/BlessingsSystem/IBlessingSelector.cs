using System.Collections.Generic;
using LineWars.Model;

namespace LineWars.Controllers
{
    // типа лимитированный список
    public interface IBlessingSelector: IReadOnlyCollection<BlessingId>
    {
        public BlessingId this[int index] { get; set; }
        public new int Count { get; set; }
    }
}