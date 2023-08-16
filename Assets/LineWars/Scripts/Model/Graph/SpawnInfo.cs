using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    /// <summary>
    /// Вся информация о изначальной принадлежности
    /// </summary>
    public class SpawnInfo
    {
         public readonly int PlayerIndex;
         public readonly Spawn SpawnNode;
         public readonly List<NodeInitialInfo> NodeInfos;

         public SpawnInfo(int playerIndex, Spawn spawnNode, IEnumerable<NodeInitialInfo> nodeInfos)
         {
             PlayerIndex = playerIndex;
             SpawnNode = spawnNode;
             NodeInfos = nodeInfos.ToList();
         }
    }
}