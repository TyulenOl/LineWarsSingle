using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "WinOrLoseActions/InfinityGameWinOrLoseActions")]
    public class InfinityGameWinOrLoseActions: WinOrLoseAction
    {
        public override void OnWin()
        {
            if (!GameVariables.IsNormalStart) return;
            
            GameRoot.Instance.UserController.PassingGameModes += 1;
            var money = GetMoneyAfterBattle();
            var diamonds = GetDiamondsAfterBattle();
            GameRoot.Instance.UserController.UserGold += money;
            GameRoot.Instance.UserController.UserDiamond += diamonds;
            WinOrLoseScene.Load(true, money, diamonds);
        }

        public override void OnLose()
        {
            if (!GameVariables.IsNormalStart) return;
            
            var money = GetMoneyAfterBattle() / 10;
            GameRoot.Instance.UserController.UserGold += money;
            WinOrLoseScene.Load(false, money, 0);
        }
    }
}