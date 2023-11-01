using UnityEngine;

namespace LineWars.Model
{
    public class TestActor : BasePlayer
    {
        private PhaseType currentPhase;
        private StateMachine stateMachine;

        private TestActorIdleState idleState;
        private TestActorBuyState buyState;
        private TestActorArtilleryState artilleryState;
        private TestActorFightState fightState;
        private TestActorScoutState scoutState;
        private TestActorReplenishState replenishState;
        
        [SerializeField] private int artilleryUnits;
        [SerializeField] private int fightUnits;
        [SerializeField] private int scoutUnits;

        [Header("Units Left")]
        [ReadOnlyInspector] public int ArtilleryLeft;
        [ReadOnlyInspector] public int FightLeft;
        [ReadOnlyInspector] public int ScoutLeft;

        [Header("Buy Options")]
        [SerializeField] private float minBuyTime;
        [SerializeField] private float maxBuyTime;

        [Header("Other Phase Options")]
        [SerializeField] private float minOtherTime;
        [SerializeField] private float maxOtherTime;
        [SerializeField] private int minUnitNum;
        [SerializeField] private int maxUnitNum;

        #region Attributes
        public float MinBuyTime => minBuyTime;
        public float MaxBuyTime => maxBuyTime;
        public float MinOtherTime => minOtherTime;
        public float MaxOtherTime => maxOtherTime;  
        public int MinUnitNum => minUnitNum;
        public int MaxUnitNum => maxUnitNum;
        public int ArtilleryUnits => artilleryUnits;
        public int FightUnits => fightUnits;
        public int ScoutUnits => scoutUnits;
        #endregion

        protected override void Awake() 
        {
            base.Awake();
            IntitializeStateMachine();
        }

        private void IntitializeStateMachine()
        {
            stateMachine = new StateMachine();

            idleState = new TestActorIdleState();
            buyState = new TestActorBuyState(this, SetNewPhase);
            artilleryState = new TestActorArtilleryState(this, SetNewPhase);
            fightState = new TestActorFightState(this, SetNewPhase);
            scoutState = new TestActorScoutState(this, SetNewPhase);
            replenishState = new TestActorReplenishState(this, SetNewPhase);

        }

        private void SetNewPhase(PhaseType phaseType)
        {
            currentPhase = phaseType;
        }
        
        #region Turns
        public override void ExecuteBuy()
        {
            stateMachine.SetState(buyState);
        }

        public override void ExecuteArtillery()
        {
            stateMachine.SetState(artilleryState);
        }

        public override void ExecuteFight()
        {
            stateMachine.SetState(fightState);
        }

        public override void ExecuteScout()
        {
            stateMachine.SetState(scoutState);
        }
        
        public override void ExecuteIdle()
        {
            stateMachine.SetState(idleState);
        }

        public override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            stateMachine.SetState(replenishState);
        }
        #endregion
        
        #region Check Turns
        public override bool CanExecuteBuy() => true;

        public override bool CanExecuteArtillery() => ArtilleryLeft > 0;
    
        public override bool CanExecuteFight() => FightLeft > 0;

        public override bool CanExecuteScout() => ScoutLeft > 0;
        public override bool CanExecuteReplenish() => true;
        #endregion
    }
}
