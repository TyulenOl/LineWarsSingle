using UnityEngine;
using System;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerExecutorState : State
        {
            private CommandsManager manager;

            public CommandsManagerExecutorState(CommandsManager manager)
            {
                this.manager = manager;
            }

            public override void OnEnter()
            {
                manager.state = CommandsManagerStateType.Executor;
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject previousObject, GameObject newObject)
            {
                if (!newObject.TryGetComponent(out IExecutor executor)) return;
                
                if (!newObject.TryGetComponent(out Owned owned)
                    || !Player.LocalPlayer.IsMyOwn(owned)) return;
                
                if (executor is Unit unit
                    && !Player.LocalPlayer.PotentialExecutors.Contains(unit.Type))
                    return;
                if (!executor.CanDoAnyAction)
                    return;

                manager.Executor = executor;

                manager.stateMachine.SetState(manager.targetState);
            }
        }
    }
}