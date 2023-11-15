using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerWaitingCommandState : CommandsManagerState
        {
            public CommandsManagerWaitingCommandState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.state = CommandsManagerStateType.WaitingSelectCommand;
                Debug.Log("Ождание выбора");
            }
        }
    }
}