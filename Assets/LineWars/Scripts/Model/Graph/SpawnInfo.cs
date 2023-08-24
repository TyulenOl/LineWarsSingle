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
         public readonly List<Node> Nodes;

         public SpawnInfo(int playerIndex, Spawn spawnNode, IEnumerable<Node> nodeInfos)
         {
             PlayerIndex = playerIndex;
             SpawnNode = spawnNode;
             Nodes = nodeInfos.ToList();
         }
    }
}