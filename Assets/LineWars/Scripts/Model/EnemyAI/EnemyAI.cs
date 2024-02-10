using System;
using System.Linq;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [Header("AI Options")]
        [SerializeField] private DepthDetailsData depthDetailsData;
        [SerializeField] private AIBuyLogicData buyLogicData;
        [SerializeField] private GameEvaluator gameEvaluator;
        [SerializeField] private AILogicType AiType;

        [Header("Timing Options")]
        [SerializeField] private float actionCooldown;
        [SerializeField] private float commandPause;
        [SerializeField] private float firstCommandPause;

        private AIBuyLogic buyLogic;
        private BaseEnemyAITurnLogic turnLogic;

        public int Depth => depthDetailsData.TotalDepth;
        protected override void OnInitialized()
        {
            buyLogic = buyLogicData.CreateAILogic(this);
            switch(AiType)
            {
                case AILogicType.Async:
                    turnLogic = new EnemyAITurnLogic(this, gameEvaluator, depthDetailsData);
                    break;
                case AILogicType.Sync:
                    turnLogic = new SyncEnemyAITurnLogic(this, gameEvaluator, depthDetailsData);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override bool CanExecuteTurn(PhaseType phase)
        {
            if(!base.CanExecuteTurn(phase)) 
                return false;

            if (phase == PhaseType.Replenish)
                return true;
            if(phase == PhaseType.Buy)
                return true;
            if(phase == PhaseType.Payday) 
                return true;
            //var executors = PhaseExecutorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects)
            {
                if (!(owned is Unit unit)) continue;
                if (/*executors.Contains(unit.Type) &&*/ unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }

        public override void ExecuteTurn(PhaseType phaseType)
        {
            InvokeTurnStarted(phaseType);
            if(phaseType == PhaseType.Payday)
            {
                ExecutePayday();
                InvokeTurnEnded(phaseType);
                return;
            }
            if (phaseType == PhaseType.Replenish)
            {
                ExecuteReplenish();
                InvokeTurnEnded(phaseType);
                return;
            }
            if (phaseType == PhaseType.Buy)
            {
                buyLogic.CalculateBuy();
                InvokeTurnEnded(phaseType);
                return;
            }
            
            GameUI.Instance.SetEnemyTurn(true);
            turnLogic.Ended += OnTurnLogicEnd;
            turnLogic.Start();

            void OnTurnLogicEnd()
            {
                turnLogic.Ended -= OnTurnLogicEnd;
                InvokeTurnEnded(phaseType);
            }
        }

        public enum AILogicType
        {
            Async,
            Sync
        }
    }
}

