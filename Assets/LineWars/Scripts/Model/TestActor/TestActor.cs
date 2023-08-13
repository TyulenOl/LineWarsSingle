using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class TestActor : MonoBehaviour, IActor
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
            PhaseManager.Instance.RegisterActor(this);
        }

        public bool CanExecuteTurn(PhaseType phaseType)
        {
            switch(phaseType)
            {
                case PhaseType.Buy:
                    return true;
                case PhaseType.Artillery:
                    return ArtilleryLeft > 0;
                case PhaseType.Fight:
                    return FightLeft > 0;
                case PhaseType.Scout:
                    return ScoutLeft > 0;
                case PhaseType.Idle:
                    return true;
            }
            return false;
        }

        public void StartTurn(PhaseType phaseType)
        {
            switch(phaseType)
            {
                case PhaseType.Buy:
                    if(!CanExecuteTurn(phaseType)) Debug.LogError($"{this} can't execute Buy Turn");
                    TurnStarted?.Invoke(phaseType);
                    stateMachine.SetState(buyState);
                    break;
                case PhaseType.Artillery:
                    if(!CanExecuteTurn(phaseType)) Debug.LogError($"{this} can't execute Artillery Turn");
                    TurnStarted?.Invoke(phaseType);
                    stateMachine.SetState(artilleryState);
                    break;
                case PhaseType.Fight:
                    if(!CanExecuteTurn(phaseType)) Debug.LogError($"{this} can't execute Fight Turn");
                    TurnStarted?.Invoke(phaseType);
                    stateMachine.SetState(fightState);
                    break;
                case PhaseType.Scout:
                    if(!CanExecuteTurn(phaseType)) Debug.LogError($"{this} can't execute Scouts Turn");
                    TurnStarted?.Invoke(phaseType);
                    stateMachine.SetState(scoutState);
                    break;
                case PhaseType.Idle:
                    TurnStarted?.Invoke(phaseType);
                    stateMachine.SetState(idleState);
                    break;
            }
        }

        public void EndTurn()
        {
            if(CurrentPhase == PhaseType.Idle)
            {
                Debug.LogWarning($"{this}: You can't end Idle Phase!");
                return;
            }
            var previousPhase = CurrentPhase;

            StartTurn(PhaseType.Idle);
            TurnEnded?.Invoke(previousPhase);
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
    }
}
