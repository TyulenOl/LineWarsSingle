using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    /// <summary>
    /// Вся информация о изначальной принадлежности
    /// </summary>
    public class SpawnInfo
    {
         public readonly int PlayerId;
         public readonly Node MainBase;
         public readonly List<Node> InitialSpawns;
         public readonly List<Node> Nodes;

         public SpawnInfo(
             int playerId,
             Node mainBase,
             IEnumerable<Node> initialSpawns,
             IEnumerable<Node> nodes)
         {
             PlayerId = playerId;
             MainBase = mainBase;
             InitialSpawns = initialSpawns.ToList();
             Nodes = nodes.ToList();
         }
    }
}