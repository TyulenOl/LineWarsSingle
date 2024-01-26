namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerWaitingExecuteState : CommandsManagerState
        {
            public CommandsManagerWaitingExecuteState(CommandsManager manager) :
                base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.State = CommandsManagerStateType.WaitingExecuteCommand;
            }
        }
    }
}