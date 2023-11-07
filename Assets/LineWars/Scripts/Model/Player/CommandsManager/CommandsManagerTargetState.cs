using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerTargetState : CommandsManagerState
        {
            private bool isCancelable;

            public CommandsManagerTargetState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                Manager.state = CommandsManagerStateType.Target;
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
                              && executor == Manager.executor))
                {
                    CancelExecutor();
                    return;
                }

                var targets = Selector.SelectedObjects
                    .Select(x => x.GetComponent<ITarget>())
                    .Where(x => x != null)
                    .ToArray();

                var targetsAndCommands = targets
                    .SelectMany(target => GetAllAvailableCommandsForPair(Manager.executor, target)
                        .Select(command => (target, command)))
                    .ToArray();
                switch (targetsAndCommands.Length)
                {
                    case 0:
                        break;
                    case 1:
                        Manager.Target = targetsAndCommands[0].target;
                        Manager.CommandExecuted.Invoke(Manager.executor, Manager.target);
                        isCancelable = false;
                        Manager.target = null;
                        UnitsController.ExecuteCommand(targetsAndCommands[0].Item2);
                        break;
                    default:
                        isCancelable = false;
                        Manager.CurrentOnWaitingCommandMessage = new OnWaitingCommandMessage(targetsAndCommands, newObject.GetComponent<Node>());
                        Manager.stateMachine.SetState(Manager.waitingCommandState);
                        break;
                }
            }

            private IEnumerable<IActionCommand> GetAllAvailableCommandsForPair(
                IExecutor executor,
                ITarget target)
            {
                if (executor is IExecutorActionSource source)
                {
                    return source.Actions
                        .OfType<ITargetedAction>()
                        .Where(x => x.IsMyTarget(target))
                        .Select(x => x.GenerateCommand(target))
                        .Where(x => x.CanExecute());
                }

                return Enumerable.Empty<IActionCommand>();
            }

            private void CancelExecutor()
            {
                if (!isCancelable) return;
                Manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                Manager.stateMachine.SetState(Manager.executorState);
            }
        }
    }
}