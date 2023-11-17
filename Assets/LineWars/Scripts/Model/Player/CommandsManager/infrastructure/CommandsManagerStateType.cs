namespace LineWars.Controllers
{
    public enum CommandsManagerStateType
    {
        Idle,
        Executor,
        Target,
        WaitingSelectCommand,
        WaitingExecuteCommand,
        MultiTarget,
        Buy
    }
}