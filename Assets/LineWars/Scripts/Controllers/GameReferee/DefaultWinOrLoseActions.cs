using UnityEngine;

namespace LineWars.Controllers
{
    
    [CreateAssetMenu(menuName = "WinOrLoseActions/DefaultWinOrLoseActions")]
    public class DefaultWinOrLoseActions: WinOrLoseAction
    {
        //TODO переделать на статистику, чем больше ящеров убил, тем больше денег

        public override int MoneyAfterBattle => Random.Range(50, 125);

        public override int DiamondsAfterBattle => Random.Range(0, 10);

        public override void OnWin()
        {
            Debug.Log("<color=yellow>Вы Победили</color>");
            if (!GameVariables.IsNormalStart)
                return;
            GameRoot.Instance.CompaniesController.WinChoseMission();
            GameRoot.Instance.CompaniesController.UnlockNextMission();
            var money = MoneyAfterBattle;
            var diamonds = DiamondsAfterBattle;
            GameRoot.Instance.UserController.UserGold += money;
            GameRoot.Instance.UserController.UserGold += diamonds;
            WinOrLoseScene.Load(true, money, diamonds);
        }

        public override void OnLose()
        {
            Debug.Log("<color=red>Потрачено</color>");
            if (!GameVariables.IsNormalStart)
                return;
            GameRoot.Instance.CompaniesController.DefeatChoseMission();
            var money = MoneyAfterBattle;
            var diamonds = DiamondsAfterBattle;
            GameRoot.Instance.UserController.UserGold += money;
            GameRoot.Instance.UserController.UserGold += diamonds;
            WinOrLoseScene.Load(false, money, diamonds);
        }
    }
}