using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using LineWars.Model;

namespace LineWars.Controllers
{
    // Client
    public partial class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }

        private ITarget target;
        private IExecutor executor;

        private StateMachine stateMachine;
        private CommandsManagerExecutorState executorState;
        private CommandsManagerTargetState targetState;
        private CommandsManagerIdleState idleState;
        private CommandsManagerWaitingCommandState waitingCommandState;
        private CommandsManagerMultiTargetState multiTargetState;

        [SerializeField, ReadOnlyInspector] private CommandsManagerStateType state;

        public UnityEvent<ITarget, ITarget> TargetChanged;
        public UnityEvent<IExecutor, IExecutor> ExecutorChanged;
        public UnityEvent<IExecutor, ITarget> CommandExecuted;

        private OnWaitingCommandMessage currentOnWaitingCommandMessage;
        public UnityEvent<OnWaitingCommandMessage> InWaitingCommandState;
        private OnWaitingCommandMessage CurrentOnWaitingCommandMessage
        {
            get => currentOnWaitingCommandMessage;
            set
            {
                InWaitingCommandState.Invoke(value);
                currentOnWaitingCommandMessage = value;
            }
        }
        
        #region Attributes

        public ITarget Target
        {
            get => target;
            private set
            {
                var previousTarget = target;
                target = value;
                if (previousTarget != target)
                    TargetChanged.Invoke(previousTarget, target);
            }
        }

        public IExecutor Executor
        {
            get => executor;
            private set
            {
                var previousExecutor = executor;
                executor = value;
                if (previousExecutor != executor)
                    ExecutorChanged.Invoke(previousExecutor, executor);
            }
        }

        public StateMachine StateMachine => stateMachine;

        #endregion

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
            Player.LocalPlayer.TurnMade.AddListener(OnTurnMade);
            Player.LocalPlayer.TurnChanged += OnTurnChanged;
        }

        private void OnEnable()
        {
            stateMachine.SetState(executorState);
        }

        private void OnDisable()
        {
            stateMachine.SetState(idleState);
            Player.LocalPlayer.TurnMade.RemoveListener(OnTurnMade);
            Player.LocalPlayer.TurnChanged -= OnTurnChanged;
        }

        private void OnTurnMade()
        {
            stateMachine.SetState(idleState);
        }

        private void OnTurnChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            if (currentPhase == PhaseType.Idle
                || stateMachine.CurrentState == executorState) return;
            stateMachine.SetState(executorState);
        }
        
        public void SelectCommand([NotNull] IActionCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (stateMachine.CurrentState != waitingCommandState)
                throw new InvalidOperationException();
            if (!currentOnWaitingCommandMessage.AllCommands.Contains(command))
                throw new ArgumentException(nameof(command));
            UnitsController.ExecuteCommand(command);
        }
    }
}