using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerWaitingSelectCommandState : CommandsManagerState
        {
            public CommandsManagerWaitingSelectCommandState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.state = CommandsManagerStateType.WaitingSelectCommand;
            }
        }
    }
}