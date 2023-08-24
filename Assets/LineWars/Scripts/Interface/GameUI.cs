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
        private IExecutor currentExecutor;
        private List<TargetDrawer> currentDrawers;
        private void Start()
        {
            Player.LocalPlayer.CurrentMoneyChanged += (PlayerOnCurrenMoneyChanged);
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
            CommandsManager.Instance.ExecutorChanged.AddListener(OnExecutorChanged);
            currentDrawers = new List<TargetDrawer>();
        }

        private void OnExecutorChanged(IExecutor arg0, IExecutor arg1)
        {
            if (arg0 != null)
            {
                arg0.ActionCompleted.RemoveListener(ReDrawCurrentTargets);
            }
            currentExecutor = arg1;
            ReDrawCurrentTargets();
            if(currentExecutor == null) return;
            currentExecutor.ActionCompleted.AddListener(ReDrawCurrentTargets);
        }

        private void ReDrawCurrentTargets()
        {
            foreach (var currentDrawer in currentDrawers)
            {
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

        private void OnDestroy()
        {
            Player.LocalPlayer.CurrentMoneyChanged -= (PlayerOnCurrenMoneyChanged);
            PhaseManager.Instance.PhaseChanged.RemoveListener(OnPhaseChanged);
        }

        private void OnPhaseChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            currentPhaseText.text = currentPhase.ToString();
        }
        void PlayerOnCurrenMoneyChanged(int before, int after)
        {
            currentMoneyText.text = $" Kоличество денег: {after}";
        }
    }
}