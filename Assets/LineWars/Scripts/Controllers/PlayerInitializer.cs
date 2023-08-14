using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class PlayerInitializer : MonoBehaviour
    {
        public T Initialize<T>(SpawnInfo spawnInfo) where T : BasePlayer
        {
            var player = new GameObject($"{typeof(T).Name}{spawnInfo.PlayerIndex}").AddComponent<T>();
            
            player.Index = spawnInfo.PlayerIndex;
            player.NationType = spawnInfo.SpawnNode.Nation;
            player.SpawnInfo = spawnInfo;
            player.CurrentMoney = spawnInfo.SpawnNode.StartMoney;
            

            foreach (var nodeInfo in spawnInfo.NodeInfos)
            {
                var node = nodeInfo.ReferenceToNode;
                Owned.Connect(player, node);

                var leftUnitPrefab = player.GetUnitPrefab(nodeInfo.LeftUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, leftUnitPrefab, UnitDirection.Left))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, leftUnitPrefab, UnitDirection.Left);
                }

                var rightUnitPrefab = player.GetUnitPrefab(nodeInfo.RightUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, rightUnitPrefab, UnitDirection.Right))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, rightUnitPrefab, UnitDirection.Right);
                }
            }

            return player;
        }
    }
}