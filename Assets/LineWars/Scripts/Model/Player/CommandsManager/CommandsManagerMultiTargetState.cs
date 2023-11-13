using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerMultiTargetState : CommandsManagerState
        {
            private IMultiTargetedAction action;
            private List<IMonoTarget> targets;

            public CommandsManagerMultiTargetState(CommandsManager manager) : base(manager)
            {
            }

            public void Prepare(
                [NotNull] IMultiTargetedAction multiTargetedAction,
                [NotNull] IMonoTarget firstTarget)
            {
                if (firstTarget == null) throw new ArgumentNullException(nameof(firstTarget));
                if (multiTargetedAction == null) throw new ArgumentNullException(nameof(multiTargetedAction));

                action = multiTargetedAction;
                targets = new List<IMonoTarget>() {firstTarget};
                Manager.SendRedrawMessage(targets, otherAction => otherAction.Equals(action));
            }

            public override void OnEnter()
            {
                Manager.state = CommandsManagerStateType.MultiTarget;
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject before, GameObject after)
            {
                var allTargets = Selector.SelectedObjects
                    .GetComponentMany<IMonoTarget>()
                    .ToArray();
                if (allTargets.Length == 0)
                    return;

                var availableTarget = allTargets
                    .FirstOrDefault(t => action.IsAvailable(targets.Concat(new[] {t}).ToArray()));

                if (availableTarget == null)
                    return;
                Manager.target = availableTarget;
                targets.Add(availableTarget);
                if (targets.Count == action.TargetsCount)
                {
                    var command = (action as IMultiTargetedActionGenerator).GenerateCommand(targets.ToArray());
                    Manager.ExecuteCommand(command);
                }
                else
                    Manager.SendRedrawMessage(targets, otherAction => otherAction.Equals(action));
            }
        }
    }
}