using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "WinOrLoseActions/InfinityGameWinOrLoseActions")]
    public class InfinityGameWinOrLoseActions: WinOrLoseAction
    {
        public override void OnWin()
        {
            if (!GameVariables.IsNormalStart) return;
            GameRoot.Instance.CompaniesController.WinChoseMission();
            WinOrLoseScene.Load(true);
        }

        public override void OnLose()
        {
            if (!GameVariables.IsNormalStart) return;
            GameRoot.Instance.CompaniesController.DefeatChoseMission();
            WinOrLoseScene.Load(false);
        }
    }
}