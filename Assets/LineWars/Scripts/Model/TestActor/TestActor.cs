using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class TestActor : MonoBehaviour//, IActor
    {
        private PhaseType currentPhase;
        private StateMachine stateMachine;

        private TestActorIdleState idleState;
        private TestActorBuyState buyState;
        private TestActorArtilleryState artilleryState;
        private TestActorFightState fightState;
        private TestActorScoutState scoutState;
        

        [SerializeField] private int artilleryUnits;
        [SerializeField] private int fightUnits;
        [SerializeField] private int scoutUnits;

        public int ArtilleryLeft;
        public int FightLeft;
        public int ScoutLeft;

        [Header("Buy Options")]
        [SerializeField] private float minBuyTime;
        [SerializeField] private float maxBuyTime;

        [Header("Other Phase Options")]
        [SerializeField] private float minOtherTime;
        [SerializeField] private float maxOtherTime;
        [SerializeField] private int minUnitNum;
        [SerializeField] private int maxUnitNum;

        public event Action<PhaseType> TurnEnded;
        public event Action<PhaseType> TurnStarted;

        #region Attributes
        public PhaseType CurrentPhase => currentPhase;
        public bool IsInTurn => CurrentPhase != PhaseType.Idle;
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

        private void Awake() 
        {
        
            IntitializeStateMachine();
        }

        private void Start() 
        {
            //PhaseManager.Instance.RegisterActor(this);
        }

        private void IntitializeStateMachine()
        {
            stateMachine = new StateMachine();

            idleState = new TestActorIdleState();
            buyState = new TestActorBuyState(this, SetNewPhase);
            artilleryState = new TestActorArtilleryState(this, SetNewPhase);
            fightState = new TestActorFightState(this, SetNewPhase);
            scoutState = new TestActorScoutState(this, SetNewPhase);

        }

        private void SetNewPhase(PhaseType phaseType)
        {
            currentPhase = phaseType;
        }

        public void ExecuteBuy()
        {
            throw new NotImplementedException();
        }

        public void ExecuteArtillery()
        {
            throw new NotImplementedException();
        }

        public void ExecuteFight()
        {
            throw new NotImplementedException();
        }

        public void ExecuteScout()
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteBuy()
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteArtillery()
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteFight()
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteScout()
        {
            throw new NotImplementedException();
        }
    }
}
