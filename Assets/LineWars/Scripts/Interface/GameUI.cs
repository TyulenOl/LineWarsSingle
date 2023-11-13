using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private EnemyTurnPanel enemyTurnPanel;
        [SerializeField] private List<Button> buttonsToBlockIfEnemyTurn;

        private List<UnitDrawer> activeUnitDrawersHash = new();

        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError($"Больше чем два {nameof(GameUI)} на сцене");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            CommandsManager.Instance.ExecutorChanged += OnExecutorChanged;
            CommandsManager.Instance.NeedRedraw += ReDrawCurrentTargets;

            SubscribeEventForGameReferee();
        }

        private void SubscribeEventForGameReferee()
        {
            if (GameReferee.Instance is ScoreReferee scoreReferee)
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
                activeUnitDrawersHash.Add(drawer);
            }
        }

        public void SetEnemyTurn(bool isEnemyTurn)
        {
            enemyTurnPanel.IsCoroutinActive = isEnemyTurn;
            foreach (var button in buttonsToBlockIfEnemyTurn)
            {
                button.interactable = !isEnemyTurn;
            }
        }

        private void ReDrawCurrentTargets(ExecutorRedrawMessage message)
        {
            foreach (var currentDrawer in currentDrawers)
            {
                if (currentDrawer == null) continue;
                currentDrawer.ReDraw(CommandType.None);
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
                    nodeTargetDrawer.ReDraw(targets.Select(x => x.CommandType));
                }
                else
                {
                    drawer.ReDraw(targetActionInfo.CommandType);
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
                drawer.ReDraw(commands);
            }
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}