using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerTargetState : State
        {
            private CommandsManager manager;
        
            public CommandsManagerTargetState(CommandsManager manager)
            {
                this.manager = manager;
            }

            public override void OnEnter()
            {
                Selector.SelectedObjectsChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectsChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
            {
                if(newObject.TryGetComponent<IExecutor>(out IExecutor executor) &&
                executor == manager.executor)
                {
                   CancelExecutor();
                   return;
                }
                if(!newObject.TryGetComponent<ITarget>(out ITarget target)) return;

                SetTarget(target);
            }

            private void SetTarget(ITarget target)
            {
                manager.Target = target;
                
                var isCommandExecuted = UnitsController.Instance.Action(manager.executor, target);

                manager.stateMachine.SetState(manager.executorState);
                if(isCommandExecuted)
                    manager.CommandExecuted.Invoke(manager.executor, manager.target);
                manager.Target = null;
                manager.Executor = null;
            }

            private void CancelExecutor()
            {
                manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                manager.stateMachine.SetState(manager.executorState);
            }
        }
    }
}

