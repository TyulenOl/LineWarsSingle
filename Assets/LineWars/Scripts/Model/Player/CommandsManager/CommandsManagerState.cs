namespace LineWars.Controllers
{
    public abstract class CommandsManagerState : State
    {
        public CommandsManager Manager { get; }
        protected CommandsManagerState(CommandsManager manager)
        {
            this.Manager = manager;
        }
    }
}