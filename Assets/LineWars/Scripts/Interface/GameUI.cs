using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private EnemyTurnPanel enemyTurnPanel;
        
        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new();
        
        private static CommandsManager CommandsManager => CommandsManager.Instance;
        

        private void Start()
        {
            CommandsManager.ExecutorChanged += OnExecutorChanged;
            CommandsManager.FightNeedRedraw += ReDrawCurrentTargets;
            CommandsManager.BuyNeedRedraw += ReDrawBuyNodes;
            
            SubscribeEventForGameReferee();
        }
        
        private void SubscribeEventForGameReferee()
        {
            if (SingleGameRoot.Instance.GameReferee is ScoreReferee scoreReferee)
            {
                scoreReferee.ScoreChanged += (player, before, after) =>
                {
                    if (player == Player.LocalPlayer)
                    {
                        scoreText.text = $"{after}/{scoreReferee.ScoreForWin}";
                    }
                };

                scoreText.text = $"{scoreReferee.GetScoreForPlayer(Player.LocalPlayer)}/{scoreReferee.ScoreForWin}";
            }
            if (SingleGameRoot.Instance.GameReferee is SiegeGameReferee siegeGameReferee)
            {
                siegeGameReferee.CurrentRoundsChanged += (currentRounds) =>
                {
                    scoreText.text = $"{currentRounds}/{siegeGameReferee.RoundsToWin}";
                };

                scoreText.text = $"{siegeGameReferee.CurrentRounds}/{siegeGameReferee.RoundsToWin}";
            }
            
            if (SingleGameRoot.Instance.GameReferee is NewDominationGameReferee dominationGameReferee)
            {
                dominationGameReferee.RoundsAmountChanged += () =>
                {
                    scoreText.text = $"{dominationGameReferee.RoundsToWin}";
                };

                scoreText.text = $"{dominationGameReferee.RoundsToWin}";
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (CommandsManager != null)
            {
                CommandsManager.ExecutorChanged += OnExecutorChanged;
                CommandsManager.FightNeedRedraw += ReDrawCurrentTargets;
                CommandsManager.BuyNeedRedraw += ReDrawBuyNodes;
            }
        }

        public void ClearBuyNodes()
        {
            ReDrawBuyNodes(null);
        }
        
        private void ReDrawBuyNodes(BuyStateMessage buyStateMessage)
        {
            if(MonoGraph.Instance == null || MonoGraph.Instance.Nodes == null)
                return;
            foreach (var node in MonoGraph.Instance.Nodes)
            {
                if(node == null)
                    continue;
                var targetDrawer = node.GetComponent<NodeTargetDrawer>();
                if(targetDrawer == null)
                    continue;
                targetDrawer.ReDrawBuyInfo(false);
            }
            
            if (buyStateMessage == null || buyStateMessage.NodesToSpawnPreset == null)
                return;
            
            foreach (var node in buyStateMessage.NodesToSpawnPreset)
            {
                if(node == null)
                    continue;
                var targetDrawer = node.GetComponent<NodeTargetDrawer>();
                if(targetDrawer == null)
                    continue;
                targetDrawer.ReDrawBuyInfo(true);
            }
        }
        
        private void OnExecutorChanged(IExecutor before, IExecutor after)
        {
            currentExecutor = after;
            ReDrawAllAvailability(before, after);
        }

        private void ReDrawAllAvailability(IExecutor before, IExecutor after)
        {
            var unitsToReDraw = Player.LocalPlayer.MyUnits;
            if (after is null)
            {
                if (before is { CanDoAnyAction: true })
                    ReDrawAllAvailability(unitsToReDraw, true);
            }
            else
            {
                ReDrawAllAvailability(unitsToReDraw, false);
            }
        }

        public void ReDrawAllAvailability(IEnumerable<Unit> units, bool isAvailable)
        {
            foreach (var unit in units)
            {
                if (!unit.CanDoAnyAction) continue;
                var drawer = unit.GetComponent<UnitDrawer>();
                drawer.ReDrawAvailability(isAvailable);
            }
        }

        public void SetEconomyTurn()
        {
            
        }
        
        public void SetEnemyTurn(bool isEnemyTurn)
        {
            enemyTurnPanel.SetTurn(isEnemyTurn);
        }

        private void ReDrawCurrentTargets(ExecutorMessage message)
        {
            foreach (var currentDrawer in currentDrawers)
            {
                if (currentDrawer == null) continue;
                currentDrawer.ReDrawCommads(CommandType.None);
            }

            if(message == null)
                return;

            currentDrawers = new List<TargetDrawer>();

            var dictionary = message.Data.GroupBy(info =>
                {
                    switch (info.Target)
                    {
                        case Node node:
                            return node;
                        case Unit unit:
                            return unit.Node;
                        default:
                            return null;
                    }
                }, targetInfo => targetInfo.CommandType,
                Tuple.Create);
            if (currentExecutor != null)
            {
                ReDrawTargetsIcons(dictionary);
            }
        }

        private void ReDrawTargetsIcons(IEnumerable<Tuple<Node, IEnumerable<CommandType>>> tuples)
        {
            foreach (var tuple in tuples)
            {
                var node = tuple.Item1;
                var commands = tuple.Item2;
                
                if (node == null) continue;
                var drawer = node.gameObject.GetComponent<NodeTargetDrawer>();
                if (drawer == null) continue;
                currentDrawers.Add(drawer);
                drawer.ReDrawCommads(commands);
            }
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}