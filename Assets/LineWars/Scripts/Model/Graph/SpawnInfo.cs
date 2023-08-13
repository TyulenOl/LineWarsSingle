using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class SpawnInfo
    {
         public readonly int PlayerIndex;
         public readonly NodeInitialInfo SpawnNode;
         public readonly List<NodeInitialInfo> NodeInfos;

         public SpawnInfo(int playerIndex, NodeInitialInfo spawnNode, IEnumerable<NodeInitialInfo> nodeInfos)
         {
             PlayerIndex = playerIndex;
             SpawnNode = spawnNode;
             NodeInfos = nodeInfos.ToList();
         }
    }
}