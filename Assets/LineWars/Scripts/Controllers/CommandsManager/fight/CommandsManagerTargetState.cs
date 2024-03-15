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
                Manager.State = CommandsManagerStateType.Target;
                Manager.SendFightRedrawMessage(Array.Empty<IMonoTarget>(), CheckAction);
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
            {
                if (!Manager.ActiveSelf)
                    return;

                if (newObject == null)
                    return;
                if (CanCancelExecutor())
                {
                    CancelExecutor();
                    return;
                }

                var presets = CollectAllOrders();
                

                if (!CanReselect())
                {
                    switch (presets.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            Manager.ProcessCommandPreset(presets[0]);
                            break;
                        default:
                            if (presets.All(preset => !preset.IsActive))
                                break;
                            Manager.GoToWaitingSelectCommandState(
                                new OnWaitingCommandMessage(
                                    presets,
                                    Selector.SelectedObjects.GetComponentMany<Node>().FirstOrDefault(),
                                    false
                                ));
                            break;
                    }
                }
                else
                {
                    switch (presets.Length)
                    {
                        case 0:
                            Reselect();
                            break;
                        default:
                            if (presets.All(preset => !preset.IsActive))
                                break;
                            Manager.GoToWaitingSelectCommandState(
                                new OnWaitingCommandMessage(
                                    presets,
                                    Selector.SelectedObjects.GetComponentMany<Node>().FirstOrDefault(),
                                    true
                                ));
                            break;
                    }
                }
            }

            private CommandPreset[] CollectAllOrders()
            {
                return Selector.SelectedObjects
                    .GetComponentMany<IMonoTarget>()
                    .SelectMany(target => GetAllActionsForPair(Manager.executor, target)
                        .Select(action =>
                            new CommandPreset(
                                Manager.Executor,
                                target,
                                action,
                                Manager.NotHaveConstraints || (
                                    Manager.Constrains.CanSelectTarget(0, target)
                                    && Manager.Constrains.IsMyCommandType(action.CommandType)
                                )
                            )))
                    .ToArray();
            }

            protected virtual IEnumerable<ITargetedAction> GetAllActionsForPair(
                IMonoExecutor executor,
                IMonoTarget target)
            {
                return executor.Actions
                    .OfType<ITargetedAction>()
                    .Where(x => x.IsAvailable(target) && CheckAction(x));
            }

            public bool CanCancelExecutor()
            {
                return Selector.SelectedObjects
                           .Any(x => x.TryGetComponent(out IMonoExecutor executor)
                                     && executor == Manager.executor)
                       && Manager.canCancelExecutor
                       && (Manager.Constrains == null || Manager.Constrains.CanCancelExecutor);
            }

            public void CancelExecutor()
            {
                Manager.Executor = null;
                Manager.stateMachine.SetState(Manager.findExecutorState);
            }

            public bool CanReselect()
            {
                return Selector.SelectedObjects
                           .Any(o =>
                               o.TryGetComponent(out IMonoExecutor executor)
                               && o.TryGetComponent(out Owned owned)
                               && Manager.Player.IsMyOwn(owned)
                               && executor != Manager.executor
                               && executor.CanDoAnyAction)
                       && Manager.canCancelExecutor
                       && (Manager.Constrains == null || Manager.Constrains.CanCancelExecutor);
            }

            public void Reselect()
            {
                var newExecutors = Selector.SelectedObjects
                    .Select(o => o.GetComponent<IMonoExecutor>())
                    .Where(x => x != null)
                    .ToArray();
                if (newExecutors.Length > 1)
                    Manager.LogError("So many executors", Manager);
                
                Manager.Executor = newExecutors[0];
                if (Manager.stateMachine.CurrentState != this)
                    Manager.stateMachine.SetState(Manager.findTargetState);
                else
                    Manager.SendFightRedrawMessage(Array.Empty<IMonoTarget>(), CheckAction);
            }

            protected virtual bool CheckAction(IExecutorAction action)
            {
                return !Manager.hiddenCommandsSet.Contains(action.CommandType);
            }
        }
    }
}