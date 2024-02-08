using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public abstract class ActionButtonLogic : MonoBehaviour
    {
        protected static CommandsManager CommandsManager => CommandsManager.Instance;
        protected Button button;

        protected virtual void Awake()
        {
            CommandsManager.StateEntered += CommandsManagerOnEnter;
            
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        
        protected virtual void CommandsManagerOnEnter(CommandsManagerStateType type)
        {
        }
        
        protected abstract void OnClick();
        
        protected static bool CanExecuteCommand(IActionCommand command)
        {
            return command != null && CommandsManager.CanExecuteAnyCommand() && command.CanExecute();
        }
    }
}