using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LineWars.Controllers
{
    public class CommandsManagerExecutorState : IState
    {
        private CommandsManager manager;
        private Action<IExecutor> setExecutor;
        public CommandsManagerExecutorState(CommandsManager manager, Action<IExecutor> setExecutor)
        {
            this.manager = manager;
            this.setExecutor = setExecutor;
        }

        public void OnEnter()
        {
            Selector.SelectedObjectsChanged += OnSelectedObjectChanged;
        }

        public void OnExit()
        {
            Selector.SelectedObjectsChanged -= OnSelectedObjectChanged;
        }

        public void OnLogic()
        {

        }

        public void OnPhysics()
        {

        }

        private void OnSelectedObjectChanged(GameObject previousObject, GameObject newObject)
        {
            if(!(newObject.TryGetComponent<IExecutor>(out IExecutor executor))) return;

            setExecutor(executor);
        }
    }
}