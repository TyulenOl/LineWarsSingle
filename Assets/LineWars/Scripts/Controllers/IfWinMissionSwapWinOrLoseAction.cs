using System;
using LineWars.Infrastructure;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(SingleGameRoot))]
    public class IfWinMissionSwapWinOrLoseAction: MonoBehaviour
    {
        [SerializeField] private WinOrLoseAction overrideWin;
        [SerializeField] private WinOrLoseAction overrideLose;
            
        private SingleGameRoot singleGameRoot;
        private void Awake()
        {
            singleGameRoot = GetComponent<SingleGameRoot>();

            if (GameRoot.Instance == null)
                return;
            var status = GameRoot.Instance.CompaniesController.ChosenMission.MissionStatus;
            if (status == MissionStatus.Completed)
            {
                if (overrideWin)
                    singleGameRoot.WinAction = overrideWin;
                if (overrideLose)
                    singleGameRoot.LoseAction = overrideLose;
            }
        }
    }
}