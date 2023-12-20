using System;
using System.Linq;
using UnityEngine;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [Header("AI Options")]
        [SerializeField] private DepthDetailsData depthDetailsData;
        [SerializeField] private AIBuyLogicData buyLogicData;
        [SerializeField] private GameEvaluator gameEvaluator;

        [Header("Timing Options")]
        [SerializeField] private float actionCooldown;
        [SerializeField] private float commandPause;
        [SerializeField] private float firstCommandPause;

        private AIBuyLogic buyLogic;
        private EnemyAITurnLogic turnLogic;

        public int Depth => depthDetailsData.TotalDepth;
        public override void Initialize(SpawnInfo spawnInfo)
        {
            base.Initialize(spawnInfo);
            buyLogic = buyLogicData.CreateAILogic(this);
            turnLogic = new EnemyAITurnLogic(this);
        }

        public void SetNewBuyLogic([NotNull] AIBuyLogicData buyData)
        {
            if (buyLogicData == null)
                Debug.LogError("Buy Logic Data cannot be null!");
            buyLogicData = buyData;
            buyLogic = buyData.CreateAILogic(this);
        }

        public void SetNewGameEvaluator([NotNull] GameEvaluator evaluator)
        {
            if (evaluator == null)
                Debug.LogError("Evaluator cannot be null!");
            gameEvaluator = evaluator;
        }

        public void SetNewDepthDetailData([NotNull] DepthDetailsData depthDetailsData)
        {
            if (depthDetailsData == null)
            {
                Debug.LogError("Depth Detail Data cannot be null!");
                return;
            }
            this.depthDetailsData = depthDetailsData;
        }

        public override bool CanExecuteTurn(PhaseType phase)
        {
            if(!base.CanExecuteTurn(phase)) //плохо? сделать явную проверку на PhaseExceptions?
                return false;

            if (phase == PhaseType.Replenish)
                return true;
            if(phase == PhaseType.Buy)
                return true;
            var executors = PhaseExecutorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects)
            {
                if (!(owned is Unit unit)) continue;
                if (executors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }

        public override void ExecuteTurn(PhaseType phaseType)
        {
            InvokeTurnStarted(phaseType);
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
            turnLogic.Ended += OnTurnLogicEnd;
            turnLogic.Start();

            void OnTurnLogicEnd()
            {
                turnLogic.Ended -= OnTurnLogicEnd;
                InvokeTurnEnded(phaseType);
            }
        }
    }
}

