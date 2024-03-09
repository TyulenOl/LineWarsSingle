using System;
using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Interface
{
    public class CommandsManagerStateListener: MonoBehaviour
    {
        private static CommandsManager CommandsManager => CommandsManager.Instance;

        [Header("Entered")]
        public UnityEvent EnteredIdle;
        public UnityEvent EnteredExecutor;
        public UnityEvent EnteredTarget;
        public UnityEvent EnteredWaitingSelectCommand;
        public UnityEvent EnteredWaitingExecuteCommand;
        public UnityEvent EnteredMultiTarget;
        public UnityEvent EnteredBuy;
        public UnityEvent EnteredCurrentCommand;
        
        [Header("Exit")]
        public UnityEvent ExitIdle;
        public UnityEvent ExitExecutor;
        public UnityEvent ExitTarget;
        public UnityEvent ExitWaitingSelectCommand;
        public UnityEvent ExitWaitingExecuteCommand;
        public UnityEvent ExitMultiTarget;
        public UnityEvent ExitBuy;
        public UnityEvent ExitCurrentCommand;


        private readonly Dictionary<CommandsManagerStateType, Action> enteredActions = new();
        private readonly Dictionary<CommandsManagerStateType, Action> exitActions = new();
        
        
        private void Start()
        {
            CommandsManager.StateEntered += CommandsManagerOnStateEntered;
            CommandsManager.StateExited += CommandsManagerOnStateExited;

            enteredActions[CommandsManagerStateType.Idle] = () => EnteredIdle?.Invoke();
            enteredActions[CommandsManagerStateType.Executor] = () => EnteredExecutor?.Invoke();
            enteredActions[CommandsManagerStateType.Target] = () => EnteredTarget?.Invoke();
            enteredActions[CommandsManagerStateType.WaitingSelectCommand] = () => EnteredWaitingSelectCommand?.Invoke();
            enteredActions[CommandsManagerStateType.WaitingExecuteCommand] = () => EnteredWaitingExecuteCommand?.Invoke();
            enteredActions[CommandsManagerStateType.MultiTarget] = () => EnteredMultiTarget?.Invoke();
            enteredActions[CommandsManagerStateType.Buy] = () => EnteredBuy?.Invoke();
            enteredActions[CommandsManagerStateType.CurrentCommand] = () => EnteredCurrentCommand?.Invoke();
            
            
            exitActions[CommandsManagerStateType.Idle] = () => ExitIdle?.Invoke();
            exitActions[CommandsManagerStateType.Executor] = () => ExitExecutor?.Invoke();
            exitActions[CommandsManagerStateType.Target] = () => ExitTarget?.Invoke();
            exitActions[CommandsManagerStateType.WaitingSelectCommand] = () => ExitWaitingSelectCommand?.Invoke();
            exitActions[CommandsManagerStateType.WaitingExecuteCommand] = () => ExitWaitingExecuteCommand?.Invoke();
            exitActions[CommandsManagerStateType.MultiTarget] = () => ExitMultiTarget?.Invoke();
            exitActions[CommandsManagerStateType.Buy] = () => ExitBuy?.Invoke();
            exitActions[CommandsManagerStateType.CurrentCommand] = () => ExitCurrentCommand?.Invoke();
        }

        private void OnDestroy()
        {
            if (CommandsManager != null)
                CommandsManager.StateEntered -= CommandsManagerOnStateEntered;
        }
        
        private void CommandsManagerOnStateEntered(CommandsManagerStateType state)
        {
            if (enteredActions.TryGetValue(state, out var action))
                action.Invoke();
        }
        
        private void CommandsManagerOnStateExited(CommandsManagerStateType state)
        {
            if (exitActions.TryGetValue(state, out var action))
                action.Invoke();
        }
    }
}