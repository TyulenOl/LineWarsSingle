using System.Collections.Generic;
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

        private List<TargetDrawer> currentTargets;

        private void Start()
        {
            Player.LocalPlayer.CurrentMoneyChanged += (PlayerOnCurrenMoneyChanged);
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
            currentTargets = new List<TargetDrawer>();
        }

        public void ReDrawTargetsIcons(List<(ITarget, CommandType)> targets)
        {
            foreach (var targetDrawer in currentTargets)
            {
                targetDrawer.ReDraw(CommandType.None);
            }
            currentTargets = new List<TargetDrawer>();
            foreach (var valueTuple in targets)
            {
                var drawerScript = valueTuple.Item1 as MonoBehaviour;
                if (drawerScript == null) continue;
                var drawer = drawerScript.gameObject.GetComponent<TargetDrawer>();
                if (drawer == null) continue;
                drawer.ReDraw(valueTuple.Item2);
                currentTargets.Add(drawer);
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