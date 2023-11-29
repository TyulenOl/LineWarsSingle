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
                if (newObject == null)
                    return;
                if (Selector.SelectedObjects
                        .Any(x => x.TryGetComponent(out IMonoExecutor executor) && executor == Manager.executor)
                    && Manager.canCancelExecutor
                    && (Manager.Constrains == null || Manager.Constrains.CanCancelExecutor))
                {
                    CancelExecutor();
                    return;
                }

                var presets = Selector.SelectedObjects
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
                    .GroupBy(x => x.Action.CommandType)
                    .Select(x => x.First())
                    .ToArray();

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
                Manager.Executor = null;
                Manager.stateMachine.SetState(Manager.findExecutorState);
            }

            protected virtual bool CheckAction(IExecutorAction action)
            {
                return !Manager.hiddenCommandsSet.Contains(action.CommandType);
            }
        }
    }
}