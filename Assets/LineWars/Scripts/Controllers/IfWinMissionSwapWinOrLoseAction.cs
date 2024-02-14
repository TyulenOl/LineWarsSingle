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
            if (GameRoot.Instance.CompaniesController.ChosenMission.MissionStatus == MissionStatus.Completed)
            {
                if (overrideWin)
                    singleGameRoot.WinAction = overrideWin;
                if (overrideLose)
                    singleGameRoot.LoseAction = overrideLose;
            }
        }
    }
}