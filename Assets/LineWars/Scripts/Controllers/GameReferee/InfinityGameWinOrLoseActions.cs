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
            WinOrLoseScene.Load(true);
        }

        public override void OnLose()
        {
            if (!GameVariables.IsNormalStart) return;
            WinOrLoseScene.Load(false);
        }
    }
}