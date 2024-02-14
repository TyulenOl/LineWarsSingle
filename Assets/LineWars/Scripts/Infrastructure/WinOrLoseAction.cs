using System;
using LineWars.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LineWars.Infrastructure
{
    [CreateAssetMenu]
    public class WinOrLoseAction : ScriptableAction
    {
        [SerializeField] private int minGold = 50;
        [SerializeField] private int maxGold = 125;
        [SerializeField] private int minDiamonds = 0;
        [SerializeField] private int maxDiamonds = 10;

        [Header("Settings")]
        [SerializeField] private bool isWin;
        [SerializeField] private bool canDoublicateGold;
        
        [SerializeField] private AddedCharacteristics addedCharacteristics;
        [SerializeField] private CompaniesActions companiesActions;

        private int GetMoneyAfterBattle() => minGold < maxGold 
            ? Random.Range(minGold, maxGold) 
            : minGold;

        private int GetDiamondsAfterBattle() => minDiamonds < maxDiamonds 
            ? Random.Range(minDiamonds, maxDiamonds) 
            : minDiamonds;
        
        public override void Execute()
        {
            if (isWin)
            {
                Debug.Log("<color=yellow>Вы Победили</color>");
            }
            else
            {
                Debug.Log("<color=red>Потрачено</color>");
            }
            
            if (GameRoot.Instance == null)
                return;
            var gold = 0;
            var diamonds = 0;

            if (addedCharacteristics.HasFlag(AddedCharacteristics.Gold))
            {
                gold = GetMoneyAfterBattle();
                GameRoot.Instance.UserController.UserGold += gold;
            }
            if (addedCharacteristics.HasFlag(AddedCharacteristics.Diamonds))
            {
                diamonds = GetDiamondsAfterBattle();
                GameRoot.Instance.UserController.UserDiamond += diamonds;
            }
            if (addedCharacteristics.HasFlag(AddedCharacteristics.PassingInfinityModes))
                GameRoot.Instance.UserController.PassingInfinityGameModes++;

            if (companiesActions.HasFlag(CompaniesActions.UnlockNextMission))
                GameRoot.Instance.CompaniesController.UnlockNextMission();
            if (companiesActions.HasFlag(CompaniesActions.LoseChoseMission))
                GameRoot.Instance.CompaniesController.DefeatChoseMissionIfNoWin();
            if (companiesActions.HasFlag(CompaniesActions.WinChoseMission))
                GameRoot.Instance.CompaniesController.WinChoseMission();
            
            WinOrLoseScene.Load(true, gold, diamonds, canDoublicateGold);
        }
    }

    [Flags]
    public enum AddedCharacteristics
    {
        Gold = 1,
        Diamonds = 2,
        PassingInfinityModes = 4
    }

    [Flags]
    public enum CompaniesActions
    {
        WinChoseMission = 1,
        LoseChoseMission = 2,
        UnlockNextMission = 4
    }
}