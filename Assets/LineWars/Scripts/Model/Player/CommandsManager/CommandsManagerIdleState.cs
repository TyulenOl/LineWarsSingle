namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerIdleState : State
        {
            private readonly CommandsManager manager;
            
            public CommandsManagerIdleState(CommandsManager manager)
            {
                this.manager = manager;
            }
            public override void OnEnter()
            {
                base.OnEnter();
                manager.state = CommandsManagerStateType.Idle;
                manager.Executor = null;
                manager.Target = null;
            }
        }
    }
}
