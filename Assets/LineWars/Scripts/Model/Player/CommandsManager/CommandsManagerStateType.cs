namespace LineWars.Controllers
{
    public enum CommandsManagerStateType
    {
        Idle,
        Executor,
        Target,
        WaitingCommand,
        MultiTarget
    }
}