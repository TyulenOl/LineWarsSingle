using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentMoneyText;
        [SerializeField] private TMP_Text currentPhaseText;
        [SerializeField] private TMP_Text currentIncomeText;
        [SerializeField] private TMP_Text scoreText;
        
        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers = new ();
        private void Start()
        {
            Player.LocalPlayer.CurrentMoneyChanged += PlayerOnCurrenMoneyChanged;
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
            
            Player.LocalPlayer.IncomeChanged += LocalPlayerOnIncomeChanged;
            LocalPlayerOnIncomeChanged(0, Player.LocalPlayer.Income);

            Player.LocalPlayer.ScoreChanged += LocalPlayerOnScoreChanged;
            LocalPlayerOnScoreChanged(0, Player.LocalPlayer.Score);
            
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            CommandsManager.Instance.ExecutorChanged.AddListener(OnExecutorChanged);
        }
        
        private void OnExecutorChanged(IExecutor before, IExecutor after)
        {
            if (before != null)
                before.ActionCompleted.RemoveListener(ReDrawCurrentTargets);
            
            currentExecutor = after;
            ReDrawCurrentTargets();
            if(currentExecutor == null) return;
            currentExecutor.ActionCompleted.AddListener(ReDrawCurrentTargets);
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
        
        private void LocalPlayerOnScoreChanged(int before, int after)
        {
            scoreText.text = $"{after} / {SingleGame.Instance.ScoreForWin}";
        }
    }
}