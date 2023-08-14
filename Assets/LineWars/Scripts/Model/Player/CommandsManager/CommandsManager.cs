using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using LineWars;
using LineWars.Model;

namespace LineWars.Controllers
{
    // Client
    public class CommandsManager: MonoBehaviour
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

        public UnityEvent TargetChanged;
        public UnityEvent ExecutorChanged;

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
            this.executor = executor;
            ExecutorChanged.Invoke();

            Debug.Log(executor);

            stateMachine.SetState(targetState);
        }

        private void SetTarget(ITarget target)
        {
            this.target = target;
            TargetChanged.Invoke();

            UnitsController.Instance.Action(executor, target);
            NullifyObjects();

            stateMachine.SetState(executorState);
        }

        private void NullifyObjects()
        {
            executor = null;
            ExecutorChanged.Invoke();

            target = null;
            TargetChanged.Invoke();
        }

    }
}