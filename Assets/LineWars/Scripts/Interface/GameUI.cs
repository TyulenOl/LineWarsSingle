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
        [SerializeField] private TMP_Text currentMoneyText;
        [SerializeField] private TMP_Text currentPhaseText;
        [SerializeField] private TMP_Text currentIncomeText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private EnemyTurnPanel enemyTurnPanel;
        [SerializeField] private List<Button> buttonsToBlockIfEnemyTurn;

        private List<UnitDrawer> activeUnitDrawersHash = new ();

        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new ();

        private void Awake()
        {
            if(Instance == null)
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
            Player.LocalPlayer.CurrentMoneyChanged += PlayerOnCurrenMoneyChanged;
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
            
            Player.LocalPlayer.IncomeChanged += LocalPlayerOnIncomeChanged;
            LocalPlayerOnIncomeChanged(0, Player.LocalPlayer.Income);
            
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            CommandsManager.Instance.ExecutorChanged.AddListener(OnExecutorChanged);
            
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
            if (before != null)
                before.ActionCompleted.RemoveListener(ReDrawCurrentTargets);
            
            currentExecutor = after;
            ReDrawCurrentTargets();
            ReDrawAllAvailability(before, after);
            if(currentExecutor == null) return;
            currentExecutor.ActionCompleted.AddListener(ReDrawCurrentTargets);
        }

        private void ReDrawAllAvailability(IExecutor before, IExecutor after)
        {
            var unitsToReDraw = Player.LocalPlayer.GetAllUnitsByPhase(PhaseManager.Instance.CurrentPhase);
            if (after is null)
            {
                if(before is { CanDoAnyAction: true })
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
                if(!unit.CanDoAnyAction) continue;
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
        
        private void ReDrawCurrentTargets()
        {
            foreach (var currentDrawer in currentDrawers)
            {
                if(currentDrawer == null) continue;
                currentDrawer.ReDraw(CommandType.None);
            }
            currentDrawers = new List<TargetDrawer>();
            if (currentExecutor != null)
            {
                ReDrawTargetsIcons(currentExecutor.GetAllAvailableTargets().ToList());
            }
        }

        private void ReDrawTargetsIcons(List<(ITarget, CommandType)> targets)
        {
            foreach (var valueTuple in targets)
            {
                var drawerScript = valueTuple.Item1 as MonoBehaviour;
                if (drawerScript == null) continue;
                var drawer = drawerScript.gameObject.GetComponent<TargetDrawer>();
                if (drawer == null) continue;
                currentDrawers.Add(drawer);
                drawer.ReDraw(valueTuple.Item2);
            }
        }
        
        private void OnPhaseChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            currentPhaseText.text = DrawHelper.GetPhaseName(currentPhase);
        }
        void PlayerOnCurrenMoneyChanged(int before, int after)
        {
            currentMoneyText.text = after.ToString();
        }
        
        private void LocalPlayerOnIncomeChanged(int before, int after)
        {
            currentIncomeText.text = after.ToString();
        }
        
        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}