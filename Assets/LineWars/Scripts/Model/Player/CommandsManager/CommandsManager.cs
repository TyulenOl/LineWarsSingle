using UnityEngine;
using UnityEngine.Events;
using LineWars.Model;

namespace LineWars.Controllers
{
    // Client
    public partial class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }
        
        private IReadOnlyTarget target;
        private IReadOnlyExecutor executor;

        private StateMachine stateMachine;
        private CommandsManagerExecutorState executorState;
        private CommandsManagerTargetState targetState;
        private CommandsManagerIdleState idleState;


        public UnityEvent<IReadOnlyTarget, IReadOnlyTarget> TargetChanged;
        public UnityEvent<IReadOnlyExecutor, IReadOnlyExecutor> ExecutorChanged;
        public UnityEvent<IReadOnlyExecutor, IReadOnlyTarget> CommandExecuted;

        #region Attributes
        public IReadOnlyTarget Target
        {
            get => target;
            private set
            {
                var previousTarget = target;
                target = value;
                TargetChanged.Invoke(previousTarget, target);
            }
        }
        public IReadOnlyExecutor Executor 
        {
            get => executor;
            private set
            {
                var previousExecutor = executor;
                executor = value;
                ExecutorChanged.Invoke(previousExecutor, executor);
            }
        }

        public StateMachine StateMachine => stateMachine;
        #endregion

        private void Awake()
        {
            if(Instance == null)
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
            if(currentPhase == PhaseType.Idle) return;
            stateMachine.SetState(executorState);
        }
    }
}