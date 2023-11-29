namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerWaitingExecuteCommandState : CommandsManagerState
        {
            public CommandsManagerWaitingExecuteCommandState(CommandsManager manager) :
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