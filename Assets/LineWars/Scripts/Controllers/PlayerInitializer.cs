using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using System.Linq;

namespace LineWars
{
    public class PlayerInitializer
    {
        public T Initialize<T>(T player, SpawnInfo spawnInfo) where T : BasePlayer
        {
            player = Object.Instantiate(player);
            player.Initialize(spawnInfo);

            foreach (var node in spawnInfo.Nodes)
            {
                Owned.Connect(player, node);

                var leftUnitPrefab = player.GetUnitPrefab(node.LeftUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, leftUnitPrefab, UnitDirection.Left))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, leftUnitPrefab, UnitDirection.Left);
                }

                var rightUnitPrefab = player.GetUnitPrefab(node.RightUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, rightUnitPrefab, UnitDirection.Right))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, rightUnitPrefab, UnitDirection.Right);
                }
            }

            return player;
        }
    }
}