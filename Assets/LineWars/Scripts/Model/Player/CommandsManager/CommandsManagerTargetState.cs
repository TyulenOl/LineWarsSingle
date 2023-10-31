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
                if(newObject.TryGetComponent(out IExecutor executor) && executor == manager.executor)
                {
                   CancelExecutor();
                   return;
                }
                if(!newObject.TryGetComponent(out ITarget target)) return;

                SetTarget(target);
            }

            private void SetTarget(ITarget target)
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

