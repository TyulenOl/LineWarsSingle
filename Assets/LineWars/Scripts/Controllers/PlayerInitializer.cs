﻿using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using System.Linq;

namespace LineWars
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private List<BasePlayer> playersPrefabs;
        public T Initialize<T>(SpawnInfo spawnInfo) where T : BasePlayer
        {
            var player = Instantiate(playersPrefabs.OfType<T>().First());
            
            player.Index = spawnInfo.PlayerIndex;
            player.NationType = spawnInfo.SpawnNode.Nation;
            player.SpawnInfo = spawnInfo;
            player.CurrentMoney = spawnInfo.SpawnNode.StartMoney;
            player.Base = spawnInfo.SpawnNode.Node;

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