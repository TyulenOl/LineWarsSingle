using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using LineWars;
using LineWars.Model;
using System.Collections.Generic;

namespace LineWars.Controllers
{
    // Client
    public class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }
        
        [Header("Debug")]
        [SerializeField] private bool isActive;
        
        private ITarget target;
        private IExecutor executor;

        private StateMachine stateMachine;
        private CommandsManagerExecutorState executorState;
        private CommandsManagerTargetState targetState;
        private CommandsManagerIdleState idleState;


        public UnityEvent<ITarget, ITarget> TargetChanged;
        public UnityEvent<IExecutor, IExecutor> ExecutorChanged;
        public UnityEvent<IExecutor, ITarget> CommandExecuted;

        #region Attributes
        public ITarget Target => target;
        public IExecutor Executor => Executor;
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
            executorState = new CommandsManagerExecutorState(this, SetExecutor);
            targetState = new CommandsManagerTargetState(this, SetTarget);
            idleState = new CommandsManagerIdleState();
        }

        private void OnEnable() 
        {
            stateMachine.SetState(executorState);
        }

        private void OnDisable()
        {
            stateMachine.SetState(idleState);
        }

        private void SetExecutor(IExecutor executor)
        {
            if(executor is Owned owned
            && !Player.LocalPlayer.OwnedObjects.Contains(owned))
                return;
            if(executor is Unit unit 
            && !Player.LocalPlayer.PotentialExecutors.Contains(unit.Type)) 
                return;
            if(executor is Unit thisUnit && Player.LocalPlayer.IsUnitUsed[thisUnit])
                return;
            
            var previousExecutor = this.executor;
            this.executor = executor;
            ExecutorChanged.Invoke(previousExecutor, this.executor);

            Debug.Log(executor);

            stateMachine.SetState(targetState);
        }

        private void SetTarget(ITarget target)
        {
            var previousTarget = this.target;
            this.target = target;
            TargetChanged.Invoke(previousTarget, this.target);
            
            var isCommandExecuted = UnitsController.Instance.Action(executor, target);
            if(isCommandExecuted)
                CommandExecuted.Invoke(executor, this.target);

            NullifyObjects();
            stateMachine.SetState(executorState);
        }

        private void NullifyObjects()
        {
            var previousExecutor = executor;
            executor = null;
            ExecutorChanged.Invoke(previousExecutor, executor);

            var previousTarget = target;
            target = null;
            TargetChanged.Invoke(previousTarget, target);
        }

    }
}