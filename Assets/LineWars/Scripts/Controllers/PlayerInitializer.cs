using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class PlayerInitializer: MonoBehaviour
    {
        [field: SerializeField] public Player Player { get; set; }
        [field: SerializeField] public List<BasePlayer> Enemies { get; set; }
        
        private int currentPlayerIndex;


        public IEnumerable<BasePlayer> InitializeAllPlayers()
        {
            yield return InitializePlayer();
            foreach (var enemy in InitializeEnemies())
                yield return enemy;
        }

        public Player InitializePlayer()
        {
            var player = InitializeBasePlayer(Player);
            player.RecalculateVisibility(false);
            return player;
        }

        public IEnumerable<BasePlayer> InitializeEnemies()
        {
            return Enemies.Select(InitializeBasePlayer);
        }
        
        private T InitializeBasePlayer<T>(T player)
            where T : BasePlayer
        {
            InitializeBasePlayer(currentPlayerIndex, player);
            currentPlayerIndex++;
            return player;
        }
        
        private static void InitializeBasePlayer(int id, BasePlayer player)
        {
            player.Initialize(id);

            foreach (var spawn in player.InitialSpawns)
            {
                spawn.IsBase = true;
            }
            
            foreach (var node in player.AllInitialNodes)
            {
                node.IsDirty = true;
                
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
        }
    }
}