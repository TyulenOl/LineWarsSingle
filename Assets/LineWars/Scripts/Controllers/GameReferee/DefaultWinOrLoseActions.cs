using UnityEngine;

namespace LineWars.Controllers
{
    
    [CreateAssetMenu(menuName = "WinOrLoseActions/DefaultWinOrLoseActions")]
    public class DefaultWinOrLoseActions: WinOrLoseAction
    {
        public override void OnWin()
        {
            Debug.Log("<color=yellow>Вы Победили</color>");
            if (!GameVariables.IsNormalStart)
                return;
            GameRoot.Instance.CompaniesController.WinChoseMission();
            GameRoot.Instance.CompaniesController.UnlockNextMission();
            
            var money = GetMoneyAfterBattle();
            var diamonds = GetDiamondsAfterBattle();
            GameRoot.Instance.UserController.UserGold += money;
            GameRoot.Instance.UserController.UserDiamond += diamonds;
            WinOrLoseScene.Load(true, money, diamonds);
        }

        public override void OnLose()
        {
            Debug.Log("<color=red>Потрачено</color>");
            if (!GameVariables.IsNormalStart)
                return;
            
            var money = GetMoneyAfterBattle() / 10;
            GameRoot.Instance.UserController.UserGold += money;
            WinOrLoseScene.Load(false, money, 0);
        }
    }
}