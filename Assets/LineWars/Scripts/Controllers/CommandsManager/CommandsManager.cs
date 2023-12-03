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
        [field:SerializeField, ReadOnlyInspector] public bool ActiveSelf { get; private set; } = true;
        
        
        [field: SerializeField] private CommandsManagerConstrainsBase Constrains { get; set; }
        public bool HaveConstrains => Constrains != null;
        public bool NotHaveConstraints => Constrains == null;

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
        private ActionInvokerBase actionInvoker;
        
        public event Action<ICommand> CommandIsExecuted;

        
        [SerializeField, ReadOnlyInspector] private CommandsManagerStateType state;

        private CommandsManagerStateType State
        {
            get => state;
            set
            {
                var previousValue = state;
                state = value;
                InvokeAction(() =>StateExited?.Invoke(previousValue));
                InvokeAction(() =>StateEntered?.Invoke(value));
            }
        }

        public event Action<CommandsManagerStateType> StateEntered;
        public event Action<CommandsManagerStateType> StateExited;


        private Player Player => Player.LocalPlayer;
        private PhaseManager PhaseManager => PhaseManager.Instance; 


        private OnWaitingCommandMessage currentOnWaitingCommandMessage;

        private OnWaitingCommandMessage CurrentOnWaitingCommandMessage
        {
            get => currentOnWaitingCommandMessage;
            set
            {
                InvokeAction(() => InWaitingCommandState?.Invoke(value));
                currentOnWaitingCommandMessage = value;
            }
        }

        public event Action<OnWaitingCommandMessage> InWaitingCommandState;


        public event Action<ExecutorRedrawMessage> FightNeedRedraw;
        public event Action<BuyStateMessage> BuyNeedRedraw;


        public IMonoTarget Target
        {
            get => target;
            private set
            {
                var previousTarget = target;
                target = value;
                if (previousTarget != value)
                    actionInvoker.Invoke(() => TargetChanged?.Invoke(previousTarget, value));
            }
        }

        public event Action<IMonoTarget, IMonoTarget> TargetChanged;


        public IMonoExecutor Executor
        {
            get => executor;
            private set
            {
                var previousExecutor = executor;
                executor = value;
                if (previousExecutor != value)
                    InvokeAction(() => ExecutorChanged?.Invoke(previousExecutor, value));
            }
        }

        public event Action<IMonoExecutor, IMonoExecutor> ExecutorChanged;


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

            actionInvoker = new GameObject(nameof(DelayInvoker)).AddComponent<DelayInvoker>();
            actionInvoker.transform.SetParent(transform);
        }

        private void Start()
        {
            Player.TurnChanged += OnTurnChanged;
        }

        private void OnDestroy()
        {
            stateMachine.SetState(idleState);
            Player.TurnChanged -= OnTurnChanged;
        }

        public bool CanExecuteAnyCommand()
        {
            return stateMachine.CurrentState == findTargetState && Executor.CanDoAnyAction;
        }

        public void ExecuteSimpleCommand(IActionCommand command)
        {
            if (HaveConstrains
                && (!Constrains.CanExecuteSimpleAction()
                    || !Constrains.IsMyCommandType(command.Action.CommandType)))
            {
                Debug.LogError($"Нельзя исполнить простую команду ввиду ограничения");
                return;
            }

            if (!CanExecuteAnyCommand())
                throw new InvalidOperationException(
                    $"В текущем состоянии командс менеджера {State} нельзя исполнить команду");
            ExecuteCommandButIgnoreConstrains(command);
        }

        private void ExecuteCommandButIgnoreConstrains(IActionCommand command)
        {
            canCancelExecutor = false;
            stateMachine.SetState(waitingExecuteCommandState);
            var action = command.Action;
            action.ActionCompleted += OnActionCompleted;
            UnitsController.ExecuteCommand(command);

            void OnActionCompleted()
            {
                action.ActionCompleted -= OnActionCompleted;
                InvokeAction(() => CommandIsExecuted?.Invoke(command));
                if (Executor as MonoBehaviour == null || !Executor.CanDoAnyAction)
                {
                    Player.FinishTurn();
                }
                else
                {
                    SendFightRedrawMessage();
                    stateMachine.SetState(findTargetState);
                }
            }
        }


        public void SetUnitPreset(UnitBuyPreset preset)
        {
            if (HaveConstrains && !Constrains.CanSelectUnitBuyPreset(preset))
            {
                Debug.LogError("Нельзя выбрать текущий пресет ввиду огрничения");
                return;
            }

            if (stateMachine.CurrentState != buyState)
                throw new InvalidOperationException("Can't set unit preset while not in buy state!");
            buyState.SetUnitPreset(preset);
        }

        public void SelectCommandsPreset(CommandPreset preset)
        {
            if (stateMachine.CurrentState != waitingSelectCommandState)
                throw new InvalidOperationException();
            if (!currentOnWaitingCommandMessage.Data.Contains(preset))
                throw new ArgumentException(nameof(preset));
            if (!preset.IsActive)
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
            if (HaveConstrains && !Constrains.CanSelectCurrentCommand())
            {
                Debug.LogError("Нельзя выбрать конкретную команду ввиду ограничения");
                return;
            }

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
            if (!ActiveSelf)
            {
                return;
            }
            ToPhase(currentPhase);
        }

        private void ToPhase(PhaseType phaseType)
        {
            switch (phaseType)
            {
                case PhaseType.Idle:
                {
                    if (Executor is {CanDoAnyAction: true})
                    {
                        Debug.LogWarning("Вы как-то завершили ход, хотя у текущего executora остались очки действия");
                    }

                    stateMachine.SetState(idleState);
                    break;
                }
                case PhaseType.Buy when Player.CanExecuteTurn(phaseType):
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
            if (!preset.IsActive) return;

            switch (preset.Action)
            {
                case IMultiTargetedAction targetedAction:
                {
                    Target = preset.Target;
                    multiTargetState.Prepare(targetedAction, preset.Target);
                    stateMachine.SetState(multiTargetState);
                    break;
                }
                case ITargetedActionCommandGenerator generator:
                {
                    Target = preset.Target;
                    var command = generator.GenerateCommand(preset.Target);
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
            InvokeAction(() => FightNeedRedraw?.Invoke(message));
        }

        private void SendFightClearMassage()
        {
            InvokeAction(() =>FightNeedRedraw?.Invoke(null));
        }

        private void SendBuyReDrawMessage(IEnumerable<Node> nodes)
        {
            InvokeAction(() => BuyNeedRedraw?.Invoke(new BuyStateMessage(nodes)));
        }

        private void SendBuyClearReDrawMessage()
        {
            InvokeAction(() => BuyNeedRedraw?.Invoke(null));
        }

        private void GoToWaitingSelectCommandState(OnWaitingCommandMessage commandMessage)
        {
            CurrentOnWaitingCommandMessage = commandMessage;
            stateMachine.SetState(waitingSelectCommandState);
        }

        private void InvokeAction(Action action)
        {
            actionInvoker.Invoke(action);
        }


        public void Activate()
        {
            if (ActiveSelf)
                return;
            ActiveSelf = true;
            ToPhase(PhaseManager.CurrentPhase);
        }
        
        public void Deactivate()
        {
            if (!ActiveSelf)
                return;
            ActiveSelf = false;
            ToPhase(PhaseType.Idle);
        }
    }
}