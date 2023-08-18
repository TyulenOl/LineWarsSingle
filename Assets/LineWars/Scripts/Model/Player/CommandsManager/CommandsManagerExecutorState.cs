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
                Selector.SelectedObjectsChanged += OnSelectedObjectChanged;
            }

            public override void OnExit()
            {
                Selector.SelectedObjectsChanged -= OnSelectedObjectChanged;
            }

            private void OnSelectedObjectChanged(GameObject previousObject, GameObject newObject)
            {
                if(!(newObject.TryGetComponent<Owned>(out Owned owned))) return;
                if(!Player.LocalPlayer.IsMyOwn(owned)) return;
                if(!(newObject.TryGetComponent<IExecutor>(out IExecutor executor))) return;
        
                SetExecutor(executor);
            }

            private void SetExecutor(IExecutor executor)
            {
                if(executor is Owned owned
                && !Player.LocalPlayer.OwnedObjects.Contains(owned))
                    return;
                if(executor is Unit unit 
                && !Player.LocalPlayer.PotentialExecutors.Contains(unit.Type)) 
                    return;
                if(executor is Unit thisUnit && Player.LocalPlayer.UnitUsage[thisUnit])
                    return;
                
                manager.Executor = executor;

                manager.stateMachine.SetState(manager.targetState);
            }
        }
    }
}