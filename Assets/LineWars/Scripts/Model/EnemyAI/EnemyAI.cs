using System;
using System.Linq;
using UnityEngine;
using JetBrains.Annotations;


namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [SerializeField] private EnemyDifficulty difficulty;
        [SerializeField] private float actionCooldown;
        [SerializeField] private AIBuyLogicData buyLogicData;
        [SerializeField] private GameEvaluator gameEvaluator;
        [field: SerializeField] public int Depth { get; set; }
        [SerializeField] private float commandPause;
        [SerializeField] private float firstCommandPause;

        private AIBuyLogic buyLogic;

        public override void Initialize(SpawnInfo spawnInfo)
        {
            base.Initialize(spawnInfo);
            buyLogic = buyLogicData.CreateAILogic(this);
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

        public override bool CanExecuteTurn(PhaseType phase)
        {
            if (phase == PhaseType.Replenish)
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

        public override ITurnLogic GetTurnLogic(PhaseType phaseType)
        {
            if (phaseType == PhaseType.Replenish)
                return new SimpleTurnLogic(ExecuteReplenish);
            if (phaseType == PhaseType.Buy)
                return new SimpleTurnLogic(buyLogic.CalculateBuy);
            return new EnemyAITurnLogic(this);
        }
    }
}

