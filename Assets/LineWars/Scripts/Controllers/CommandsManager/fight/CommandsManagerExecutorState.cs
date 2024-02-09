using UnityEngine;
using System;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerFindExecutorState : CommandsManagerState
        {
            public CommandsManagerFindExecutorState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                Manager.State = CommandsManagerStateType.Executor;
                Manager.SendFightClearMassage();
                Selector.SelectedObjectChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject previousObject, GameObject newObject)
            {
                if (!Manager.ActiveSelf)
                    return;
                
                if (!newObject.TryGetComponent(out IMonoExecutor executor)) return;

                if (!newObject.TryGetComponent(out Owned owned)
                    || !Player.LocalPlayer.IsMyOwn(owned)) return;

                // if (executor is Unit unit
                //     && !Player.LocalPlayer.PotentialExecutors.Contains(unit.Type))
                //     return;
                if (!executor.CanDoAnyAction)
                    return;

                if (Manager.HaveConstrains && !Manager.Constrains.CanSelectExecutor(executor))
                    return;

                Manager.Executor = executor;
                Manager.stateMachine.SetState(Manager.findTargetState);
            }
        }
    }
}