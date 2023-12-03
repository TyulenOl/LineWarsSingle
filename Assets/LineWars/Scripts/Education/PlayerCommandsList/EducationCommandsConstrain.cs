using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Education
{
    [RequireComponent(typeof(CommandsManager))]
    public class EducationCommandsConstrain : CommandsManagerConstrainsBase
    {
        [SerializeField] private PlayerActions playerActions;
        private PlayerAction currentAction;

        private bool isFinish;
        [field: SerializeField] public UnityEvent EducationFinished { get; private set; }
        [field: SerializeField] public UnityEvent TurnFinished { get; private set; }

        private CommandsManager commandsManager;

        private void Awake()
        {
            commandsManager = GetComponent<CommandsManager>();
            if (playerActions.ContainsNext())
                currentAction = playerActions.GetNext();
            else
                Debug.LogError("The player hasn't any actions");
        }

        private void OnEnable()
        {
            commandsManager.CommandIsExecuted += CommandsManagerOnCommandIsExecuted;
#if UNITY_EDITOR
            TurnFinished.AddListener(() => Debug.Log("TurnFinished"));
            EducationFinished.AddListener(() => Debug.Log("EducationFinished"));          
#endif
        }

        private void OnDisable()
        {
            commandsManager.CommandIsExecuted -= CommandsManagerOnCommandIsExecuted;
        }

        private void CommandsManagerOnCommandIsExecuted(ICommand command)
        {
            currentAction = playerActions.FindNext();
            if (currentAction is FinishTurnPlayerAction)
            {
                TurnFinished?.Invoke();
                currentAction = playerActions.FindNext();
            }

            if (!isFinish && currentAction == null)
            {
                isFinish = true;
                EducationFinished?.Invoke();
            }
        }

        public override bool CanCancelExecutor => currentAction != null && currentAction.CanCancelExecutor;

        public override bool CanSelectExecutor(IMonoExecutor executor)
        {
            return CheckCurrentAction() && currentAction.CanSelectExecutor(executor);
        }

        public override bool CanSelectTarget(int targetId, IMonoTarget target)
        {
            return CheckCurrentAction() && currentAction.CanSelectTarget(targetId, target);
        }

        public override bool IsMyCommandType(CommandType commandType)
        {
            return CheckCurrentAction() && currentAction.IsMyCommandType(commandType);
        }

        public override bool CanExecuteSimpleAction()
        {
            return CheckCurrentAction() && currentAction.CanExecuteSimpleAction();
        }

        public override bool CanSelectCurrentCommand()
        {
            return CheckCurrentAction() && currentAction.CanSelectCurrentCommand();
        }

        public override bool CanSelectNode(Node node)
        {
            return CheckCurrentAction() && currentAction.CanSelectNode(node);
        }

        public override bool CanSelectUnitBuyPreset(UnitBuyPreset preset)
        {
            return CheckCurrentAction() && currentAction.CanSelectUnitBuyPreset(preset);
        }

        private bool CheckCurrentAction()
        {
            if (currentAction == null)
            {
                Debug.LogError("The training actions are over");
                return false;
            }

            return true;
        }
    }
}