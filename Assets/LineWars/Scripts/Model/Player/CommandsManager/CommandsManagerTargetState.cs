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
            private bool isCancelable;
        
            public CommandsManagerTargetState(CommandsManager manager)
            {
                this.manager = manager;
            }

            public override void OnEnter()
            {
                Selector.SelectedObjectsChanged += OnSelectedObjectChanged;
                isCancelable = true;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectsChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
            {
                if(newObject.TryGetComponent<IReadOnlyExecutor>(out IReadOnlyExecutor executor) &&
                executor == manager.executor)
                {
                   CancelExecutor();
                   return;
                }
                if(!newObject.TryGetComponent<IReadOnlyTarget>(out IReadOnlyTarget target)) return;

                SetTarget(target);
            }

            private void SetTarget(IReadOnlyTarget target)
            {
                manager.Target = target;
                var isCommandExecuted = UnitsController.Instance.Action(manager.executor, target);
                if(isCommandExecuted)
                {
                    manager.CommandExecuted.Invoke(manager.executor, manager.target);
                    isCancelable = false;
                }
                    
                manager.Target = null;
            }

            private void CancelExecutor()
            {
                if(!isCancelable) return;
                manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                manager.stateMachine.SetState(manager.executorState);
            }
        }
    }
}

