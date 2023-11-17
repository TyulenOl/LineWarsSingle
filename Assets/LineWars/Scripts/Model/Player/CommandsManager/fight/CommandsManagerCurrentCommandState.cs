using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerCurrentCommandState: CommandsManagerFindTargetState
        {
            public CommandType CommandType { get; private set; }
            public CommandsManagerCurrentCommandState(CommandsManager manager) : base(manager)
            {
            }

            public void Prepare(CommandType commandType)
            {
                CommandType = commandType;
            }

            protected override bool CheckAction(IExecutorAction action)
            {
                return action.CommandType == CommandType;
            }
        }
    }
}