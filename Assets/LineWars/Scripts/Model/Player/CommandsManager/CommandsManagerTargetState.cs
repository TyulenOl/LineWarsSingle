using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public class CommandsManagerTargetState : State
    {
        private CommandsManager manager;
        private Action<ITarget> setTarget;
    
        public CommandsManagerTargetState(CommandsManager manager, Action<ITarget> setTarget)
        {
            this.manager = manager;
            this.setTarget = setTarget;
        }

        public override void OnEnter()
        {
            Selector.SelectedObjectsChanged += OnSelectedObjectChanged;
            //Debug.Log("Starting");
        }

        public override void OnExit()
        {
            Selector.SelectedObjectsChanged -= OnSelectedObjectChanged;
        }

        public override void OnLogic()
        {
            
        }


        private void OnSelectedObjectChanged(GameObject lastObject, GameObject newObject)
        {
            if(!(newObject.TryGetComponent<ITarget>(out ITarget target))) return;

            setTarget(target);
        }
    }
}

