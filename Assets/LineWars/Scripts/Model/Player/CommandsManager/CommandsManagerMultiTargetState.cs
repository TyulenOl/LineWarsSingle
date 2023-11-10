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
            private List<ITarget> targets;
            private int targetId;
            
            public CommandsManagerMultiTargetState(CommandsManager manager) : base(manager)
            {
            }

            public void Prepare(
                [NotNull] IMultiTargetedAction multiTargetedAction,
                [NotNull] ITarget firstTarget)
            {
                if (firstTarget == null) throw new ArgumentNullException(nameof(firstTarget));
                if (action == null) throw new ArgumentNullException(nameof(multiTargetedAction));

                action = multiTargetedAction;
                targets = new List<ITarget>(){firstTarget};
                targetId = 1;
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
                    .GetComponentMany<ITarget>()
                    .ToArray();
                if (allTargets.Length == 0)
                    return;

                var availableTarget = allTargets
                    .FirstOrDefault(t => action.IsAvailable(targetId, t));
                
                if (availableTarget == null)
                    return;
                targets.Add(availableTarget);
                targetId++;
                if (targetId == action.TargetsCount)
                {
                    var command = (action as IMultiTargetedActionGenerator).GenerateCommand(targets.ToArray());
                    UnitsController.ExecuteCommand(command);
                }
            }
        }
    }
}