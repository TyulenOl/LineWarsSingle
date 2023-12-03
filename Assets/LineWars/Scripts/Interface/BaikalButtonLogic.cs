using LineWars.Controllers;
using LineWars.LineWars.Scripts.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BaikalButtonLogic : ActionButtonLogic
    {
        protected override void OnClick()
        {
            var executor = CommandsManager.Instance.Executor;
            if (executor is Unit unit)
            {
                var command = new HealYourSelfCommand<Node, Edge, Unit>(unit);
                if (command.CanExecute())
                    CommandsManager.Instance.ExecuteSimpleCommand(command);
            }
        }
    }
}   