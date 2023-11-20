using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }

        private IMonoTarget target;
        private IMonoExecutor executor;
        private bool canCancelExecutor = true;

        private StateMachine stateMachine;
        private CommandsManagerIdleState idleState;
        private CommandsManagerFindExecutorState findExecutorState;
        private CommandsManagerFindTargetState findTargetState;
        private CommandsManagerWaitingSelectCommandState waitingSelectCommandState;
        private CommandsManagerWaitingExecuteCommandState waitingExecuteCommandState;
        private CommandsManagerMultiTargetState multiTargetState;
        private CommandsManagerCurrentCommandState currentCommandState;

        private CommandsManagerBuyState buyState;

        
        [SerializeField] private List<CommandType> hiddenCommands;

        private HashSet<CommandType> hiddenCommandsSet;
        public event Action<IMonoTarget, IMonoTarget> TargetChanged;
        public event Action<IMonoExecutor, IMonoExecutor> ExecutorChanged;
        
        private OnWaitingCommandMessage currentOnWaitingCommandMessage;
        public event Action<OnWaitingCommandMessage> InWaitingCommandState;
        
        

        [SerializeField, ReadOnlyInspector] private CommandsManagerStateType state;
        private CommandsManagerStateType State
        {
            get => state;
            set
            {
                var previousValue = state;
                state = value;
                StateExited?.Invoke(previousValue);
                StateEntered?.Invoke(state);
            }
        }

        public event Action<CommandsManagerStateType> StateEntered;
        public event Action<CommandsManagerStateType> StateExited; 

        
        
        
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

        public event Action<ExecutorRedrawMessage> FightNeedRedraw;
        public event Action<BuyStateMessage> BuyNeedRedraw;

        public bool CommandIsExecuted => stateMachine.CurrentState == waitingExecuteCommandState;

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
            findExecutorState = new CommandsManagerFindExecutorState(this);
            findTargetState = new CommandsManagerFindTargetState(this);
            idleState = new CommandsManagerIdleState(this);
            waitingSelectCommandState = new CommandsManagerWaitingSelectCommandState(this);
            waitingExecuteCommandState = new CommandsManagerWaitingExecuteCommandState(this);
            multiTargetState = new CommandsManagerMultiTargetState(this);

            currentCommandState = new CommandsManagerCurrentCommandState(this);

            buyState = new CommandsManagerBuyState(this);

            hiddenCommandsSet = hiddenCommands.ToHashSet();
        }

        private void Start()
        {
            stateMachine.SetState(findExecutorState);
            Player.LocalPlayer.TurnChanged += OnTurnChanged;
        }

        private void OnDestroy()
        {
            stateMachine.SetState(idleState);
            Player.LocalPlayer.TurnChanged -= OnTurnChanged;
        }

        public bool CanExecuteAnyCommand()
        {
            return stateMachine.CurrentState == findTargetState && Executor.CanDoAnyAction;
        }

        public void ExecuteCommand(IActionCommand command)
        {
            if (!CanExecuteAnyCommand())
                throw new InvalidOperationException($"В текущем состоянии командс менеджера {State}" +
                                                    $" нельзя исполнить команду");
            ExecuteCommandButIgnoreConstrains(command);
        }

        private void ExecuteCommandButIgnoreConstrains(IActionCommand command)
        {
            if (Executor as MonoBehaviour == null)
                Debug.LogError("EXECUTOR is missing");
            canCancelExecutor = false;
            stateMachine.SetState(waitingExecuteCommandState);
            var action = command.Action;
            action.ActionCompleted += OnActionCompleted;
            UnitsController.ExecuteCommand(command);

            void OnActionCompleted()
            {
                if (!Executor.CanDoAnyAction)
                {
                    Player.FinishTurn();
                }
                else
                {
                    SendFightRedrawMessage();
                    stateMachine.SetState(findTargetState);
                }

                action.ActionCompleted -= OnActionCompleted;
            }
        }


        public void SetUnitPreset(UnitBuyPreset preset)
        {
            if (stateMachine.CurrentState != buyState)
                Debug.LogError("Can't set unit preset while not in buy state!");
            buyState.SetUnitPreset(preset);
        }

        public void SelectCommandsPreset(CommandPreset preset)
        {
            if (stateMachine.CurrentState != waitingSelectCommandState)
                throw new InvalidOperationException();
            if (!currentOnWaitingCommandMessage.Data.Contains(preset))
                throw new ArgumentException(nameof(preset));
            ProcessCommandPreset(preset);
        }

        public void CancelCommandPreset()
        {
            if (stateMachine.CurrentState != waitingSelectCommandState)
            {
                throw new InvalidOperationException("Is not targeted state to cancelAction");
            }
            stateMachine.SetState(findTargetState);
        }

        public void SelectCurrentCommand(CommandType commandType)
        {
            if (stateMachine.CurrentState != findTargetState)
                throw new InvalidOperationException();
            if (CheckContainsActions(commandType))
                throw new InvalidOperationException();
            if (Executor.Actions.First(x => x.CommandType == commandType) is not ITargetedAction)
                throw new NotImplementedException();
            currentCommandState.Prepare(commandType);
            stateMachine.SetState(currentCommandState);

            bool CheckContainsActions(CommandType commandType)
            {
                return !Executor.Actions
                    .Select(x => x.CommandType)
                    .Contains(commandType);
            }
        }

        public void CancelCurrentCommand()
        {
            if (stateMachine.CurrentState != currentCommandState)
                throw new InvalidOperationException();
            stateMachine.SetState(findTargetState);
        }

        private void OnTurnChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            switch (currentPhase)
            {
                case PhaseType.Idle:
                {
                    if (Executor is {CanDoAnyAction: true})
                    {
                        Debug.LogError("Вы как-то завершили ход, хотя у текущего executora остались очки действия");
                    }

                    stateMachine.SetState(idleState);
                    break;
                }
                case PhaseType.Buy when Player.CanExecuteTurn(currentPhase):
                {
                    stateMachine.SetState(buyState);
                    break;
                }
                default:
                {
                    stateMachine.SetState(findExecutorState);
                    break;
                }
            }
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
                    ExecuteCommandButIgnoreConstrains(command);
                    break;
                }
                default: throw new Exception();
            }
        }

        private void SendFightRedrawMessage(
            IEnumerable<IMonoTarget> targets = null,
            Func<IExecutorAction, bool> actionSelector = null)
        {
            var visitor = new GetAllAvailableTargetActionInfoForMonoExecutorVisitor(
                new GetAvailableTargetActionInfoVisitor.ForShotUnitAction(targets ?? Array.Empty<IMonoTarget>()),
                actionSelector ?? (action => !hiddenCommandsSet.Contains(action.CommandType)));
            var data = Executor.Accept(visitor).ToArray();
            var message = new ExecutorRedrawMessage(data);
            FightNeedRedraw?.Invoke(message);
        }

        private void SendFightClearMassage()
        {
            FightNeedRedraw?.Invoke(null);
        }

        private void SendBuyReDrawMessage(IEnumerable<Node> nodes)
        {
            BuyNeedRedraw?.Invoke(new BuyStateMessage(nodes));
        }

        private void ClearBuyReDrawMessage()
        {
            BuyNeedRedraw?.Invoke(null);
        }

        private void GoToWaitingSelectCommandState(OnWaitingCommandMessage commandMessage)
        {
            CurrentOnWaitingCommandMessage = commandMessage;
            stateMachine.SetState(waitingSelectCommandState);
        }
    }
}