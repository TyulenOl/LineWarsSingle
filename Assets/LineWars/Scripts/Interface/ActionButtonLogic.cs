using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.LineWars.Scripts.Interface
{
    public abstract class ActionButtonLogic : MonoBehaviour
    {
        protected Button button;

        private void Awake()
        {
            CommandsManager.Instance.StateEntered += ReDrawOnEnter;
            CommandsManager.Instance.StateExited += ReDrawOnExit;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();
        
        private void ReDrawOnEnter(CommandsManagerStateType type)
        {
            switch (type)
            {
                case CommandsManagerStateType.WaitingSelectCommand:
                case CommandsManagerStateType.WaitingExecuteCommand:
                case CommandsManagerStateType.CurrentCommand:
                case CommandsManagerStateType.MultiTarget:
                    button.interactable = false;
                    break;
            }
        }

        private void ReDrawOnExit(CommandsManagerStateType type)
        {
            switch (type)
            {
                case CommandsManagerStateType.WaitingSelectCommand:
                case CommandsManagerStateType.WaitingExecuteCommand:
                case CommandsManagerStateType.CurrentCommand:
                case CommandsManagerStateType.MultiTarget:
                    button.interactable = true;
                    break;
            }
        }
    }
}