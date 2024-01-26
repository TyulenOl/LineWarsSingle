using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private EnemyTurnPanel enemyTurnPanel;
        [SerializeField] private List<Button> buttonsToBlockIfEnemyTurn;

        private List<UnitDrawer> activeUnitDrawersСash = new();

        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new();
        

        private void Start()
        {
            CommandsManager.Instance.ExecutorChanged += OnExecutorChanged;
            CommandsManager.Instance.FightNeedRedraw += ReDrawCurrentTargets;
            CommandsManager.Instance.BuyNeedRedraw += ReDrawBuyNodes;
            
            SubscribeEventForGameReferee();
        }

        public void ClearBuyNodes()
        {
            ReDrawBuyNodes(null);
        }
        
        private void ReDrawBuyNodes(BuyStateMessage buyStateMessage)
        {
            foreach (var node in MonoGraph.Instance.Nodes)
            {
                node.GetComponent<NodeTargetDrawer>().ReDrawBuyInfo(false);
            }
            
            if (buyStateMessage == null)
                return;
            
            foreach (var node in buyStateMessage.NodesToSpawnPreset)
            {
                node.GetComponent<NodeTargetDrawer>().ReDrawBuyInfo(true);
            }
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

        private void OnExecutorChanged(IExecutor before, IExecutor after)
        {
            currentExecutor = after;
            ReDrawAllAvailability(before, after);
        }

        private void ReDrawAllAvailability(IExecutor before, IExecutor after)
        {
            var unitsToReDraw = Player.LocalPlayer.GetAllUnitsByPhase(PhaseManager.Instance.CurrentPhase);
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
                activeUnitDrawersСash.Add(drawer);
            }
        }

        public void SetEnemyTurn(bool isEnemyTurn)
        {
            enemyTurnPanel.IsCoroutinActive = isEnemyTurn;
            foreach (var button in buttonsToBlockIfEnemyTurn)
            {
                button.gameObject.SetActive(!isEnemyTurn);
            }
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

        [Obsolete]
        private void ReDrawTargetsIcons(IEnumerable<TargetActionInfo> targets)
        {
            foreach (var targetActionInfo in targets)
            {
                var drawerScript = targetActionInfo.Target as MonoBehaviour;
                if (drawerScript == null) continue;
                var drawer = drawerScript.gameObject.GetComponent<TargetDrawer>();
                if (drawer == null) continue;
                currentDrawers.Add(drawer);

                if (drawer is NodeTargetDrawer nodeTargetDrawer)
                {
                    nodeTargetDrawer.ReDrawCommads(targets.Select(x => x.CommandType));
                }
                else
                {
                    drawer.ReDrawCommads(targetActionInfo.CommandType);
                }
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