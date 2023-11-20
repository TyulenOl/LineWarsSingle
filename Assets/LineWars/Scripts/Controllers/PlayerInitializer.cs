using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using System.Linq;

namespace LineWars
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private List<BasePlayer> playersPrefabs;

        [Header("AI Options")]
        [SerializeField] private GameEvaluator gameEvaluator;
        [SerializeField] private AIBuyLogicData aiBuyLogicData;
        [SerializeField] private int aiDepth;

        public T Initialize<T>(SpawnInfo spawnInfo) where T : BasePlayer
        {
            var player = Instantiate(playersPrefabs.OfType<T>().First());
            
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

            //слабейший костыль
            if(player is EnemyAI enemyAI)
                InitializeAISettings(enemyAI);
            
            return player;
        }

        private void InitializeAISettings(EnemyAI enemyAI)
        {
            if(gameEvaluator != null)
                enemyAI.SetNewGameEvaluator(gameEvaluator);
            if(aiBuyLogicData != null)
                enemyAI.SetNewBuyLogic(aiBuyLogicData);
            if (aiDepth > 0)
                enemyAI.Depth = aiDepth;
        }
    }
}