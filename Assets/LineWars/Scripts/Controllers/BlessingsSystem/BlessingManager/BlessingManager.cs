using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Controllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LineWars.Model
{
    // объединить с CommandsManager?
    public sealed partial class BlessingManager: MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<BlessingData, BaseBlessing> dataToBlessingDictionary;

        private StateMachine stateMachine;
        private BlessingManagerIdleState idleState;
        private BlessingManagerWaitingChoseState waitingChoseState;
        private BlessingManagerWaitingExecuteState waitingExecuteState;
        
        
        [SerializeField] private BlessingManagerStateType stateType;
        
        private Dictionary<BlessingData, int> blessingAndCount;
        private CommandsManager commandsManager;

        public IEnumerable<(BlessingData, int)> BlessingDatas
            => blessingAndCount.Select(x => (x.Key, x.Value));

        public event Action<BlessingData, int> BlessingCountChanged; 
        public event Action<BlessingData> BlessingStarted; 
        public event Action<BlessingData> BlessingCompleted;

        private Coroutine delayCoroutine;
        
        public void Initialize(
            CommandsManager commandsManager,
            BlessingInitialData blessingInitialData)
        {
            this.commandsManager = commandsManager;
            blessingAndCount = blessingInitialData.BlessingDataAndCount
                .ToDictionary(x => x.Item1, x => x.Item2);

            stateMachine = new StateMachine();
            idleState = new BlessingManagerIdleState(this, BlessingManagerStateType.Idle);
            waitingChoseState = new BlessingManagerWaitingChoseState(this, BlessingManagerStateType.WaitingChose);
            waitingExecuteState = new BlessingManagerWaitingExecuteState(this, BlessingManagerStateType.WaitingExecute);
            
            this.commandsManager.StateEntered += CommandsManagerOnStateEntered;
        }

        private void CommandsManagerOnStateEntered(CommandsManagerStateType state)
        {
            if (stateType is BlessingManagerStateType.WaitingExecute)
                Debug.LogError("Неправильное поведение для BlessingManager");
            
            if (state is not CommandsManagerStateType.Executor)
            {
                if (stateType != BlessingManagerStateType.Idle)
                    stateMachine.SetState(idleState);
            }
            else
            {
                if (stateType != BlessingManagerStateType.WaitingChose)
                    stateMachine.SetState(waitingChoseState);
            }
        }
        
        public bool CanExecuteBlessing(BlessingData blessingData)
        {
            return stateType == BlessingManagerStateType.WaitingChose
                   && blessingAndCount.TryGetValue(blessingData, out var count)
                   && count > 0
                   && dataToBlessingDictionary.TryGetValue(blessingData, out var blessing)
                   && blessing.CanExecute();
        }
        
        public void ExecuteBlessing(BlessingData blessingData)
        {
            if (stateType != BlessingManagerStateType.WaitingChose)
            {
                InvalidStateLog(nameof(ExecuteBlessing));
                return;
            }
            
            var blessing = dataToBlessingDictionary[blessingData];
            blessing.Completed += OnBlissingComplete;
        }

        private void OnBlissingComplete()
        {
            
        }

        private IEnumerator DelayBlissingCoroutine()
        {
            
        }
        
        private void InvalidStateLog(string methodName)
        {
            LogError($"This is invalid state {stateType} for action {methodName}", gameObject);
        }
        
        private void LogError(string log, Object obj)
        {
            if (commandsManager.NeedErrorLog)
                Debug.LogError($"<color=yellow>{CommandsManager.CommandsManagerPrefix}</color> {log}", obj);
        }
    }
}