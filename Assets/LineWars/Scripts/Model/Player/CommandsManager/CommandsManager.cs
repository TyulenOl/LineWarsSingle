using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    // Client
    public partial class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }

        private IMonoTarget target;
        private IMonoExecutor executor;
        private bool canCancelExecutor = true;

        private StateMachine stateMachine;
        private CommandsManagerExecutorState executorState;
        private CommandsManagerTargetState targetState;
        private CommandsManagerIdleState idleState;
        private CommandsManagerWaitingCommandState waitingCommandState;
        private CommandsManagerMultiTargetState multiTargetState;

        [SerializeField, ReadOnlyInspector] private CommandsManagerStateType state;
        public event Action<IMonoTarget, IMonoTarget> TargetChanged;
        public event Action<IMonoExecutor, IMonoExecutor> ExecutorChanged;


        private OnWaitingCommandMessage currentOnWaitingCommandMessage;
        public event Action<OnWaitingCommandMessage> InWaitingCommandState;
        
        private Player Player => Player.LocalPlayer;
        private OnWaitingCommandMessage CurrentOnWaitingCommandMessage
        {
            get => currentOnWaitingCommandMessage;
            set
            {
                InWaitingCommandState?.Invoke(value);
                currentOnWaitingCommandMessage = value;
            }
        }

        public event Action<ExecutorRedrawMessage> NeedRedraw;


        public IMonoTarget Target
        {
            get => target;
            private set
            {
                var previousTarget = target;
                target = value;
                if (previousTarget != target)
                    TargetChanged?.Invoke(previousTarget, target);
            }
        }

        public IMonoExecutor Executor
        {
            get => executor;
            private set
            {
                var previousExecutor = executor;
                executor = value;
                if (previousExecutor != executor)
                    ExecutorChanged?.Invoke(previousExecutor, executor);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Больше чем два CommandsManager на сцене");
            }

            stateMachine = new StateMachine();
            executorState = new CommandsManagerExecutorState(this);
            targetState = new CommandsManagerTargetState(this);
            idleState = new CommandsManagerIdleState(this);
            waitingCommandState = new CommandsManagerWaitingCommandState(this);
            multiTargetState = new CommandsManagerMultiTargetState(this);
        }
        
        private void Start()
        {
            stateMachine.SetState(executorState);
            Player.LocalPlayer.TurnChanged += OnTurnChanged;
        }

        private void OnDestroy()
        {
            stateMachine.SetState(idleState);
            Player.LocalPlayer.TurnChanged -= OnTurnChanged;
        }

        public void ExecuteCommand(ICommand command)
        {
            UnitsController.ExecuteCommand(command);
            if (!Executor.CanDoAnyAction)
            {
                FinishTurn();
            }
            else 
            {
                SendRedrawMessage(Array.Empty<IMonoTarget>());
                stateMachine.SetState(targetState);
            }
        }
        
        public void SelectCommandsPreset(CommandPreset preset)
        {
            if (stateMachine.CurrentState != waitingCommandState)
                throw new InvalidOperationException();
            if (!currentOnWaitingCommandMessage.Data.Contains(preset))
                throw new ArgumentException(nameof(preset));
            ProcessCommandPreset(preset);
        }

        private void OnTurnChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            if (currentPhase == PhaseType.Idle
                || stateMachine.CurrentState == executorState) return;
            stateMachine.SetState(executorState);
        }
        
        private void ProcessCommandPreset(CommandPreset preset)
        {
            var presetExecutor = preset.Executor;
            var presetAction = preset.Action;
            var presetTarget = preset.Target;
            switch (presetAction)
            {
                case IMultiTargetedAction targetedAction:
                {
                    Target = presetTarget;
                    multiTargetState.Prepare(targetedAction, presetTarget);
                    stateMachine.SetState(multiTargetState);
                    break;
                }
                case ITargetedActionCommandGenerator generator:
                {
                    Target = presetTarget;
                    var command = generator.GenerateCommand(presetTarget);
                    ExecuteCommand(command);
                    break;
                }
                default: throw new Exception();
            }
        }

        private void SendRedrawMessage(
            IEnumerable<IMonoTarget> targets,
            Func<IUnitAction<Node, Edge, Unit>, bool> actionSelector = null)
        {
            var visitor = new GetAllAvailableTargetActionInfoForMonoExecutorVisitor(
                new GetAvailableTargetActionInfoVisitor.ForShotUnitAction(targets.ToArray()),
                actionSelector);
            var data = Executor.Accept(visitor).ToArray();
            var message = new ExecutorRedrawMessage(data);
            NeedRedraw?.Invoke(message);
        }

        private void SendClearMassage()
        {
            NeedRedraw?.Invoke(null);
        }

        private void GoToWaitingCommandState(OnWaitingCommandMessage commandMessage)
        {
            CurrentOnWaitingCommandMessage = commandMessage;
            stateMachine.SetState(waitingCommandState);
        }

        private void FinishTurn()
        {
            stateMachine.SetState(idleState);
            Player.FinishTurn();
        }
    }
}