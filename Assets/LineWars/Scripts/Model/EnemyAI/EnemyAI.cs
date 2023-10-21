using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [SerializeField] private EnemyDifficulty difficulty;
        [SerializeField] private float actionCooldown;
        [SerializeField] private EnemyAIPersonality personality;
        [SerializeField] private GameEvaluator gameEvaluator;

        private IReadOnlyExecutor currentExecutor;
        private EnemyAIBuySelector buySelector;

        protected override void Awake()
        {
            base.Awake();
            buySelector = new EnemyAIBuySelector();
        }

        #region Turns

        public override void ExecuteBuy()
        {
            StartCoroutine(BuyCoroutine());
            IEnumerator BuyCoroutine()
            {
                if (buySelector.TryGetPreset(this, out var preset))
                    SpawnPreset(preset);
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }
        public override void ExecuteArtillery() => throw new NotImplementedException();
        public override void ExecuteFight() => throw new NotImplementedException();
        public override void ExecuteScout() => throw new NotImplementedException();

        public override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            StartCoroutine(ReplenishCoroutine());
            IEnumerator ReplenishCoroutine()
            {
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }

        public void ExecuteTurn(PhaseType phase)
        {

        }


        #endregion

        #region Check Turns
        public override bool CanExecuteBuy() => true;
        public override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);
        public override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);
        public override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);
        public override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phase)
        {
            var executors = PhaseExecutorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects)
            {
                if (!(owned is Unit unit)) continue;
                if (executors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        #endregion
    }
}

