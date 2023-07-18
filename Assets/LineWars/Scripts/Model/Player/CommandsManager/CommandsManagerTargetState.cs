using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public class CommandsManagerTargetState : IState
    {
        private CommandsManager manager;
        private Action<ITarget> setTarget;
        public CommandsManagerTargetState(CommandsManager manager, Action<ITarget> setTarget)
        {
            this.manager = manager;
            this.setTarget = setTarget;
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }

        public void OnLogic()
        {
            
        }

        public void OnPhysics()
        {
            
        }

        private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
        {
            if(!(newObject.TryGetComponent<ITarget>(out ITarget target))) return;

            setTarget(target);
        }
    }
}

