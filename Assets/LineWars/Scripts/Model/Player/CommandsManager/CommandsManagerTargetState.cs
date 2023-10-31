using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerTargetState : State
        {
            private CommandsManager manager;
            private bool isCancelable;

            public CommandsManagerTargetState(CommandsManager manager)
            {
                this.manager = manager;
            }

            public override void OnEnter()
            {
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
                isCancelable = true;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectChanged -= OnSelectedObjectChanged;
                
            }

            private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
            {
                if (newObject == null)
                    return;
                if (Selector.SelectedObjects
                    .Any(x => x.TryGetComponent(out IExecutor executor)
                              && executor == manager.executor))
                {
                    CancelExecutor();
                    return;
                }

                var targets = Selector.SelectedObjects
                    .Select(x => x.GetComponent<ITarget>())
                    .Where(x => x != null)
                    .ToArray();

                var targetsAndCommands = targets
                    .Select(x => (x,GetAvailableCommandForPair(manager.executor, x)))
                    .Where(x => x.Item2 != null)
                    .ToArray();
                
                if (targetsAndCommands.Length == 0)
                {
                    
                }
                else if (targetsAndCommands.Length == 1)
                {
                    manager.Target = targetsAndCommands[0].Item1;
                    UnitsController.ExecuteCommand(targetsAndCommands[0].Item2);
                    manager.CommandExecuted.Invoke(manager.executor, manager.target);
                    isCancelable = false;
                    manager.target = null;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }


            private ICommand GetAvailableCommandForPair(IExecutor executor, ITarget target)
            {
                foreach (var commandType in target.CommandPriorityData)
                {
                    if (executor.TryGetCommandForTarget(commandType, target, out var command)
                        && command.CanExecute())
                    {
                        return command;
                    }
                }

                return null;
            }

            private void CancelExecutor()
            {
                if (!isCancelable) return;
                manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                manager.stateMachine.SetState(manager.executorState);
            }
        }
    }
}