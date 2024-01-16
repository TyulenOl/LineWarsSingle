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
            WinOrLoseScene.Load(true);
        }

        public override void OnLose()
        {
            Debug.Log("<color=red>Потрачено</color>");
            if (!GameVariables.IsNormalStart)
                return;
            GameRoot.Instance.CompaniesController.DefeatChoseMission();
            WinOrLoseScene.Load(false);
        }
    }
}