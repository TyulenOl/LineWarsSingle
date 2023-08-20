using System.Linq;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars
{
    public partial class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField] private PhaseExecutorsData phaseExecutorsData;

        private IReadOnlyCollection<UnitType> potentialExecutors;
        private bool isTurnMade;

        private StateMachine stateMachine;
        private PlayerPhase idlePhase;
        private PlayerPhase artilleryPhase;
        private PlayerPhase fightPhase;
        private PlayerPhase scoutPhase;
        private PlayerBuyPhase buyPhase;
        private PlayerReplenishPhase replenishPhase;
        
        [field: SerializeField] public UnityEvent TurnMade {get; private set;}
        public IReadOnlyCollection<UnitType> PotentialExecutors => potentialExecutors;
        public bool IsTurnMade
        {
            get => isTurnMade;
            private set
            {
                isTurnMade = value;
                if(value)
                    TurnMade.Invoke();
            }
        }


        protected override void Awake()
        {
            base.Awake();
            if (LocalPlayer != null)
                Debug.LogError("More than two players on the scene!");
            else
                LocalPlayer = this;

            
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
            idlePhase = new PlayerPhase(this, PhaseType.Idle);
            buyPhase = new PlayerBuyPhase(this, PhaseType.Buy);
            artilleryPhase = new PlayerPhase(this, PhaseType.Artillery);
            fightPhase = new PlayerPhase(this, PhaseType.Fight);
            scoutPhase = new PlayerPhase(this, PhaseType.Scout);
            replenishPhase = new PlayerReplenishPhase(this, PhaseType.Replenish);
        }

        private void OnOwnedAdded(Owned owned)
        {
            if(!(owned is Unit unit)) return;
            unit.ActionPointChanged.AddListener(ProcessActionPointsChange);
        }

        private void OnOwnerRemoved(Owned owned)
        {
            if(!(owned is Unit unit)) return;
            unit.ActionPointChanged.RemoveListener(ProcessActionPointsChange);
        }

        private void ProcessActionPointsChange(int previousValue, int currentValue)
        {
            if (currentValue <= 0 && CurrentPhase != PhaseType.Idle)
                IsTurnMade = true;
        }
        
        public void FinishTurn()
        {
            if(!IsTurnMade) return;
            ExecuteTurn(PhaseType.Idle);
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

            foreach (var owned in OwnedObjects)
            {
                if(!(owned is Unit unit)) continue;
                if(phaseExecutors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }

        #endregion
    }
}