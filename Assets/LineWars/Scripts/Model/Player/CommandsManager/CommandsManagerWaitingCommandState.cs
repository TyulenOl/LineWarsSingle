using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerWaitingCommandState : CommandsManagerState
        {
            public CommandsManagerWaitingCommandState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.state = CommandsManagerStateType.WaitingCommand;
                Debug.Log("Ождание выбора");
            }
        }
    }
}