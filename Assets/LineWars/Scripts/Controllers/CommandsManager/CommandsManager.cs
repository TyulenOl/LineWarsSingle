using System;
using System.Collections;
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

        [SerializeField] private float maxActionDelayInSeconds = 5;
        
        public bool HaveConstrains => Constrains != null;
        public bool NotHaveConstraints => Constrains == null;

        private IMonoTarget target;
        private IMonoExecutor executor;
        private bool canCancelExecutor = true;
        
        private IActionCommand currentExecutedCommand;
        private Coroutine delayActionCoroutine;

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
                if (previousExecutor != null)
                {
                    previousExecutor.ExecutorDestroyed -= OnExecutorDestroy;
                }

                if (executor != null)
                {
                    executor.ExecutorDestroyed += OnExecutorDestroy;
                }
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
                Debug.LogError($"More than two {nameof(CommandsManager)} on stage");
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
            Player.TurnEnded += OnTurnEnded;
            Player.TurnStarted += OnTurnStarted;
        }

        private void OnDestroy()
        {
            stateMachine.SetState(idleState);
            Player.TurnEnded -= OnTurnStarted;
            Player.TurnStarted -= OnTurnStarted;
        }

        public bool CanExecuteAnyCommand()
        {
            return stateMachine.CurrentState == findTargetState && Executor.CanDoAnyAction;
        }

        public bool CanSetExecutor()
        {
            return ActiveSelf &&
                   (stateMachine.CurrentState == findExecutorState || stateMachine.CurrentState == findTargetState) &&
                   (!HaveConstrains || Constrains.CanCancelExecutor) &&
                   canCancelExecutor;
        }
        

        public void SetExecutor(IMonoExecutor executor)
        {
            if (executor == null)
                throw new ArgumentNullException(nameof(executor));
            
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(SetExecutor));
                return;
            }

            if (stateMachine.CurrentState != findExecutorState && stateMachine.CurrentState != findTargetState)
            {
                InvalidStateLog(nameof(SetExecutor));
                return;
            }

            if (stateMachine.CurrentState == findTargetState && HaveConstrains && !Constrains.CanCancelExecutor)
            {
                ConstrainsLog(nameof(SetExecutor));
                return;
            }

            if (!canCancelExecutor)
            {
                Debug.LogError("You cannot change the executor");
                return;
            }

            if (stateMachine.CurrentState == findExecutorState)
            {
                Executor = executor;
                stateMachine.SetState(findTargetState);
                return;
            }
            
            Executor = executor;
            SendFightRedrawMessage(
                Array.Empty<IMonoTarget>(), 
                action => !hiddenCommandsSet.Contains(action.CommandType)
            );
        }

        public void ExecuteSimpleCommand(IActionCommand command)
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(ExecuteSimpleCommand));
                return;
            }
            
            if (HaveConstrains
                && (!Constrains.CanExecuteSimpleAction()
                    || !Constrains.IsMyCommandType(command.Action.CommandType)))
            {
                ConstrainsLog(nameof(ExecuteSimpleCommand));
                return;
            }

            if (!CanExecuteAnyCommand())
            {
                InvalidStateLog(nameof(ExecuteSimpleCommand));
                return;
            }
            
            ExecuteCommandButIgnoreConstrains(command);
        }

        private void ExecuteCommandButIgnoreConstrains(IActionCommand command)
        {
            currentExecutedCommand = command;
            canCancelExecutor = false;
            stateMachine.SetState(waitingExecuteCommandState);
            
            command.Action.ActionCompleted += OnActionCompleted;
            delayActionCoroutine = StartCoroutine(DelayActionCoroutine());
            
            UnitsController.ExecuteCommand(command);
        }
        
        void OnActionCompleted()
        {
            currentExecutedCommand.Action.ActionCompleted -= OnActionCompleted;
            StopCoroutine(delayActionCoroutine);
            delayActionCoroutine = null;
            currentExecutedCommand = null;
            
            InvokeAction(() => CommandIsExecuted?.Invoke(currentExecutedCommand));
            if (Executor as MonoBehaviour == null)
            {
                Debug.LogError("Impossible behavior!");
                return;
            }
            if (!Executor.CanDoAnyAction)
            {
                Player.FinishTurn();
            }
            else
            {
                SendFightRedrawMessage();
                stateMachine.SetState(findTargetState);
            }
        }

        IEnumerator DelayActionCoroutine()
        {
            yield return new WaitForSeconds(maxActionDelayInSeconds);
            Debug.LogWarning($"The action didn't stop after {maxActionDelayInSeconds} seconds!");
            OnActionCompleted();
        }

        private void OnExecutorDestroy()
        {
            Debug.Log("EXECUTOR DESTROY");
            if (delayActionCoroutine != null)
                StopCoroutine(delayActionCoroutine);
            if (currentExecutedCommand != null)
            {
                currentExecutedCommand.Action.ActionCompleted -= OnActionCompleted;
                InvokeAction(() => CommandIsExecuted?.Invoke(currentExecutedCommand));
            }
            delayActionCoroutine = null;
            currentExecutedCommand = null;
            
            Player.FinishTurn();
        }

        public void SetDeckCard(DeckCard deckCard)
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(SetDeckCard));
                return;
            }
            
            if (HaveConstrains && !Constrains.CanSelectDeckCard(deckCard))
            {
                ConstrainsLog(nameof(SetDeckCard));
                return;
            }

            if (stateMachine.CurrentState != buyState)
            {
                InvalidStateLog(nameof(SetDeckCard));
                return;
            }
            
            buyState.SetDeckCard(deckCard);
        }

        public bool SelectCommandsPreset(CommandPreset preset)
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(SelectCommandsPreset));
                return false;
            }
            
            if (stateMachine.CurrentState != waitingSelectCommandState)
            {
                InvalidStateLog(nameof(SelectCommandsPreset));
                return false;
            }

            if (!currentOnWaitingCommandMessage.Data.Contains(preset))
            {
                Debug.LogError($"You cant select this command preset because this command preset in not owned!", gameObject);
                return false;
            }
            
            if (!preset.IsActive)
            {
                Debug.LogError("You cant select this command preset because this command preset is not active!", gameObject);
                return false;
            }
            ProcessCommandPreset(preset);
            return true;
        }

        public void CancelCommandPreset()
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(CancelCommandPreset));
                return;
            }
            
            if (stateMachine.CurrentState != waitingSelectCommandState)
            {
                InvalidStateLog(nameof(CancelCommandPreset));
                return;
            }

            stateMachine.SetState(findTargetState);
        }

        public bool SelectCurrentCommand(CommandType commandType)
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(SelectCurrentCommand));
                return false;
            }
            
            if (HaveConstrains && !Constrains.CanSelectCurrentCommand())
            {
                ConstrainsLog(nameof(SelectCurrentCommand));
                return false;
            }

            if (stateMachine.CurrentState != findTargetState)
            {
                InvalidStateLog(nameof(SelectCurrentCommand));
                return false;
            }

            if (CheckContainsActions(commandType))
            {
                Debug.LogError($"You cant {nameof(SelectCurrentCommand)} because {nameof(Executor)} will not be able to perform this");
                return false;
            }
            if (Executor.Actions.First(x => x.CommandType == commandType) is not ITargetedAction)
                throw new NotImplementedException();
            currentCommandState.Prepare(commandType);
            stateMachine.SetState(currentCommandState);

            return true;
            
            bool CheckContainsActions(CommandType commandType)
            {
                return !Executor.Actions
                    .Select(x => x.CommandType)
                    .Contains(commandType);
            }
        }

        public void CancelCurrentCommand()
        {
            if (!ActiveSelf)
            {
                ActiveSelfLog(nameof(CancelCurrentCommand));
                return;
            }
            
            if (stateMachine.CurrentState != currentCommandState)
            {
                InvalidStateLog(nameof(CancelCurrentCommand));
                return;
            }
            stateMachine.SetState(findTargetState);
        }

        private void OnTurnEnded(IActor _, PhaseType phaseType)
        {
            if (Executor != null && Executor as MonoBehaviour != null && Executor.CanDoAnyAction)
            {
                Debug.LogWarning("You somehow ended a turn even though the current executor still has action points", gameObject);
            }
            
            stateMachine.SetState(idleState);
        }

        private void OnTurnStarted(IActor _, PhaseType phaseType)
        {
            ToPhase(phaseType);
        }

        private void ToPhase(PhaseType phaseType)
        {
            switch (phaseType)
            {
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
        }
        
        public void Deactivate()
        {
            if (!ActiveSelf)
                return;
            ActiveSelf = false;
        }


        private void ConstrainsLog(string methodName)
        {
            Debug.LogError($"Cant invoke {methodName} because you have constrains {Constrains.name}.", gameObject);
        }

        private void InvalidStateLog(string methodName)
        {
            Debug.LogError($"This is invalid state {state} for action {methodName}", gameObject);
        }

        private void ActiveSelfLog(string methodName)
        {
            Debug.LogError($"Cant execute {methodName} because {nameof(CommandsManager)} is no active!", gameObject);
        }
    }
}