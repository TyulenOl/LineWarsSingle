using LineWars.Model;
using System.Collections.Generic;

namespace LineWars.Controllers
{
    public static class BlessingHelper
    {
        public static IEnumerable<BlessingId> FindBlessingsByType(this IStorage<BlessingId, BaseBlessing> storage, Rarity rarity)
        {
            foreach(var blessing in storage.Values)
            {
                if(blessing.BlessingId.Rarity == rarity)
                    yield return blessing.BlessingId;
            }
        }
    }
}
