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
                    .GetComponentMany<ITarget>()
                    .ToArray();

                var presets = targets
                    .SelectMany(target => GetAllActionsForPair(Manager.executor, target)
                        .Select(action => new CommandPreset(target, action)))
                    .ToArray();

                switch (presets.Length)
                {
                    case 0:
                        break;
                    case 1:
                        isCancelable = false;
                        Manager.ProcessTargetedAction(presets[0]);
                        break;
                    default:
                        isCancelable = false;
                        Manager.CurrentOnWaitingCommandMessage =
                            new OnWaitingCommandMessage(
                                presets,
                                Selector.SelectedObjects.GetComponentMany<Node>().FirstOrDefault()
                            );
                        Manager.stateMachine.SetState(Manager.waitingCommandState);
                        break;
                }
            }

            private IEnumerable<ITargetedAction> GetAllActionsForPair(
                IExecutor executor,
                ITarget target)
            {
                if (executor is IExecutorActionSource source)
                {
                    return source.Actions
                        .OfType<ITargetedAction>()
                        .Where(x => x.IsAvailable(target));
                }

                return Enumerable.Empty<ITargetedAction>();
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