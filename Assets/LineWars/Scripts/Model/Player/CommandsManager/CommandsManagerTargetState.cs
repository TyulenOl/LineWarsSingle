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
            public CommandsManagerTargetState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                Manager.state = CommandsManagerStateType.Target;
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
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
                    .Any(x => x.TryGetComponent(out IMonoExecutor executor)
                              && executor == Manager.executor))
                {
                    CancelExecutor();
                    return;
                }

                var targets = Selector.SelectedObjects
                    .GetComponentMany<IMonoTarget>()
                    .ToArray();

                var presets = targets
                    .SelectMany(target => GetAllActionsForPair(Manager.executor, target)
                        .Select(action => new CommandPreset(Manager.Executor, target, action)))
                    .ToArray();

                switch (presets.Length)
                {
                    case 0:
                        break;
                    case 1:
                        Manager.canCancelExecutor = false;
                        Manager.ProcessCommandPreset(presets[0]);
                        break;
                    default:
                        Manager.canCancelExecutor = false;
                        Manager.GoToWaitingCommandState(
                            new OnWaitingCommandMessage(
                                presets,
                                Selector.SelectedObjects.GetComponentMany<Node>().FirstOrDefault()
                            ));
                        break;
                }
            }

            private IEnumerable<ITargetedAction> GetAllActionsForPair(
                IMonoExecutor executor,
                IMonoTarget target)
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
                if (!Manager.canCancelExecutor) return;
                Manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                Manager.stateMachine.SetState(Manager.executorState);
            }
        }
    }
}