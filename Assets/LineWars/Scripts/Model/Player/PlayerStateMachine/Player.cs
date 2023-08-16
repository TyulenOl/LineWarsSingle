using System.Linq;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField] private PhaseExecutorsData phaseExecutorsData;

        private IReadOnlyCollection<UnitType> potentialExecutors;
        private Dictionary<Unit, bool> isUnitUsed;

        private StateMachine stateMachine;
        private PlayerPhase idlePhase;
        private PlayerPhase artilleryPhase;
        private PlayerPhase fightPhase;
        private PlayerPhase scoutPhase;
        private PlayerBuyPhase buyPhase;
        private PlayerReplenishPhase replenishPhase;

        public IReadOnlyCollection<UnitType> PotentialExecutors => potentialExecutors;
        public IReadOnlyDictionary<Unit, bool> IsUnitUsed => isUnitUsed;


        protected override void Awake()
        {
            base.Awake();
            if (LocalPlayer != null)
                Debug.LogError("More than two players on the scene!");
            else
                LocalPlayer = this;

            isUnitUsed = new Dictionary<Unit, bool>();
            InitializeStateMachine();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OwnedAdded += OnOwnedAdded;
            OwnedRemoved += OnOwnerRemoved;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OwnedAdded -= OnOwnedAdded;
            OwnedRemoved -= OnOwnerRemoved;
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            idlePhase = new PlayerPhase(this, PhaseType.Idle, SetExecutors, SetUnitUsed);
            buyPhase = new PlayerBuyPhase(this, PhaseType.Buy, SetExecutors, SetUnitUsed);
            artilleryPhase = new PlayerPhase(this, PhaseType.Artillery, SetExecutors, SetUnitUsed);
            fightPhase = new PlayerPhase(this, PhaseType.Fight, SetExecutors, SetUnitUsed);
            scoutPhase = new PlayerPhase(this, PhaseType.Scout, SetExecutors, SetUnitUsed);
            replenishPhase = new PlayerReplenishPhase(this, PhaseType.Replenish, SetExecutors, SetUnitUsed);
        }

        private void SetExecutors(PhaseType phaseType)
        {
            potentialExecutors = phaseExecutorsData.PhaseToUnits[phaseType];
        }

        private void OnOwnedAdded(Owned owned)
        {
            if (owned is Unit unit)
            {
                isUnitUsed[unit] = false;
            }
        }

        private void OnOwnerRemoved(Owned owned)
        {
            if (owned is Unit unit)
            {
                isUnitUsed.Remove(unit);
            }
        }

        private void SetUnitUsed(Unit unit, bool value)
        {
            isUnitUsed[unit] = value;
        }


        #region Turns

        public override void ExecuteBuy()
        {
            stateMachine.SetState(buyPhase);
        }

        public override void ExecuteArtillery()
        {
            stateMachine.SetState(artilleryPhase);
        }

        public override void ExecuteFight()
        {
            stateMachine.SetState(fightPhase);
        }

        public override void ExecuteScout()
        {
            stateMachine.SetState(scoutPhase);
        }

        public override void ExecuteIdle()
        {
            stateMachine.SetState(idlePhase);
        }

        public override void ExecuteReplenish()
        {
            stateMachine.SetState(replenishPhase);
        }

        #endregion

        #region Check Turns

        public override bool CanExecuteBuy() => true;

        public override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);

        public override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);

        public override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);

        public override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phaseType)
        {
            var phaseExecutors = phaseExecutorsData.PhaseToUnits[phaseType];

            foreach (var unitData in isUnitUsed)
            {
                if (!unitData.Value && phaseExecutors.Contains(unitData.Key.Type))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}