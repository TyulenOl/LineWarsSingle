using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerFindTargetState : CommandsManagerState
        {

            public CommandsManagerFindTargetState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                Manager.state = CommandsManagerStateType.Target;
                Manager.SendFightRedrawMessage(Array.Empty<IMonoTarget>(), CheckAction);
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
                        Manager.GoToWaitingSelectCommandState(
                            new OnWaitingCommandMessage(
                                presets,
                                Selector.SelectedObjects.GetComponentMany<Node>().FirstOrDefault()
                            ));
                        break;
                }
            }

            protected virtual IEnumerable<ITargetedAction> GetAllActionsForPair(
                IMonoExecutor executor,
                IMonoTarget target)
            {
                return executor.Actions
                    .OfType<ITargetedAction>()
                    .Where(x => x.IsAvailable(target) 
                                && CheckAction(x));
            }

            private void CancelExecutor()
            {
                if (!Manager.canCancelExecutor) return;
                Manager.Executor = null;
                Debug.Log("EXECUTOR CANCELED");

                Manager.stateMachine.SetState(Manager.findExecutorState);
            }

            protected virtual bool CheckAction(IExecutorAction action)
            {
                return !Manager.hiddenCommandsSet.Contains(action.CommandType);
            }
        }
    }
}