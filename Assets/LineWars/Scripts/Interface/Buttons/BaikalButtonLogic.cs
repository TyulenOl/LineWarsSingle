using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BaikalButtonLogic : ActionButtonLogic
    {
        protected override void OnClick()
        {
            var command = GenerateCommand();
            if (CanExecuteCommand(command))
            {
                CommandsManager.ExecuteSimpleCommand(command);
            }
        }

        protected override void CommandsManagerOnEnter(CommandsManagerStateType type)
        {
            base.CommandsManagerOnEnter(type);
            button.interactable = CanExecuteCommand(GenerateCommand());
        }

        private static IActionCommand GenerateCommand()
        {
            if (CommandsManager.Executor is Unit unit)
                return new HealYourSelfCommand<Node, Edge, Unit>(unit);
            return null;
        }
    }
}   