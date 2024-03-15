namespace LineWars.Controllers
{
    public class GameSpaceAction: HotKeyAction
    {
        public override void Invoke()
        {
            if (CommandsManager.Instance != null 
                && CommandsManager.Instance.CanCancelExecutor()
                && !PauseInstaller.Paused)
            {
                CommandsManager.Instance.CancelExecutor();
            }
        }
    }
}