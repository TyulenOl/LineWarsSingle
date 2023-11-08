using System;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        public class CommandsManagerMultiTargetState : CommandsManagerState
        {
            // следующий тип с которым может взаимодействовать игрок
            private Type FindType => action.AdditionalTargets[currentIndex];
            private int currentIndex;
            private IMultiStageTargetedAction action; 
            public CommandsManagerMultiTargetState(CommandsManager manager) : base(manager)
            {
            }

            public void Prepare(IMultiStageTargetedAction multiStageTargetedAction)
            {
                action = multiStageTargetedAction;
                currentIndex = 0;
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
                if (Selector.SelectedObjects.All(x => x.GetComponent(FindType) == null))
                    return;
                
            }
        }
    }
}