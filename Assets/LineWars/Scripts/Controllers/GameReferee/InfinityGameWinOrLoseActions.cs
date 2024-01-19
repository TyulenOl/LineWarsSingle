using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "WinOrLoseActions/InfinityGameWinOrLoseActions")]
    public class InfinityGameWinOrLoseActions: WinOrLoseAction
    {
        public override int MoneyAfterBattle => Random.Range(50, 100);

        public override int DiamondsAfterBattle => Random.Range(0, 5);
        
        public override void OnWin()
        {
            if (!GameVariables.IsNormalStart) return;
            GameRoot.Instance.UserController.PassingGameModes += 1;
            WinOrLoseScene.Load(true);
        }

        public override void OnLose()
        {
            if (!GameVariables.IsNormalStart) return;
            WinOrLoseScene.Load(false);
        }
    }
}