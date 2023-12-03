using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class BuyStateMessage
    {
        public IEnumerable<Node> NodesToSpawnPreset { get; }

        public BuyStateMessage(IEnumerable<Node> nodesToSpawnPreset)
        {
            NodesToSpawnPreset = nodesToSpawnPreset.ToArray();
        }
    }
}