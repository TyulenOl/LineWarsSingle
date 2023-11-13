namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerIdleState : CommandsManagerState
        {
            public CommandsManagerIdleState(CommandsManager manager) : base(manager)
            {
            }
            public override void OnEnter()
            {
                base.OnEnter();
                Manager.state = CommandsManagerStateType.Idle;
                Manager.Executor = null;
                Manager.Target = null;
                Manager.canCancelExecutor = true;
                Manager.SendClearMassage();
            }
        }
    }
}
