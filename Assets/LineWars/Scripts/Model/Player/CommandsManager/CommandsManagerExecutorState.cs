using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LineWars.Controllers
{
    public class CommandsManagerExecutorState : State
    {
        private CommandsManager manager;
        private Action<IExecutor> setExecutor;
        public CommandsManagerExecutorState(CommandsManager manager, Action<IExecutor> setExecutor)
        {
            this.manager = manager;
            this.setExecutor = setExecutor;
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
            if(!(newObject.TryGetComponent<IExecutor>(out IExecutor executor))) return;

            setExecutor(executor);
        }
    }
}