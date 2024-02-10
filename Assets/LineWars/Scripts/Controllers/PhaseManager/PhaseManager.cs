using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;
using UnityEngine.Events;

namespace LineWars
{  
    public partial class PhaseManager : MonoBehaviour
    {
        private List<IActor> actors;
        [field: SerializeField] public PhaseOrderData OrderData { get; protected set; }
        
        //public event Action<IActor, PhaseType, PhaseType> ActorTurnChanged;

        public UnityEvent<PhaseType> PhaseEntered;
        public UnityEvent<PhaseType> PhaseExited;

        private StateMachine stateMachine;
        private PhaseSynchronousState buyState;
        private PhaseAlternatingState artilleryState;
        private PhaseAlternatingState fightState;
        private PhaseAlternatingState scoutState;
        private PhaseSynchronousState replenishState;
        
        private Dictionary<PhaseType, Phase> typeToPhase;
        private int currentActorId;
        private bool isFirstRound = false;
        public IReadOnlyList<IActor> Actors => actors.AsReadOnly();
        public IActor CurrentActor => actors[currentActorId];
        public PhaseType CurrentPhase => ((Phase)stateMachine.CurrentState).Type;
    
        public static PhaseManager Instance {get; private set;}
        private void Awake() 
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("More than two PhaseManagers on the scene!");
            }
            actors = new List<IActor>();
            InitializeStateMachine();
            stateMachine.StateChanged += OnStateChanged;
        }

        private void Update() 
        {
            stateMachine.OnLogic();
        }

        private void FixedUpdate() 
        {
            stateMachine.OnPhysics();
        }

        public void StartGame()
        {
            Debug.Log("Game Started!");
            if(OrderData.FirstPhase != PhaseType.None)
            {
                stateMachine.SetState(typeToPhase[OrderData.FirstPhase]);
                isFirstRound = true;
                return;
            }
            stateMachine.SetState(typeToPhase[OrderData.Order[0]]);
        }

        public void RegisterActor(IActor actor)
        {
            if(actors.Contains(actor))
            {
                Debug.LogError($"{actor} is already registered!");
            }
            actors.Add(actor);
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            buyState = new PhaseSynchronousState(PhaseType.Buy, this);
            artilleryState = new PhaseAlternatingState(PhaseType.Artillery, this);
            fightState = new PhaseAlternatingState(PhaseType.Fight, this);
            scoutState = new PhaseAlternatingState(PhaseType.Scout, this);
            replenishState = new PhaseSynchronousState(PhaseType.Replenish, this);
            
            typeToPhase = new Dictionary<PhaseType, Phase>()
            {
                {PhaseType.Buy, buyState},
                {PhaseType.Artillery, artilleryState},
                {PhaseType.Fight, fightState},
                {PhaseType.Scout, scoutState},
                {PhaseType.Replenish, replenishState}
            };
        }
        
        private void ToNextPhase()
        {
            StartCoroutine(NextCoroutine());

            IEnumerator NextCoroutine()
            {
                yield return null;
                stateMachine.SetState(GetNextPhase((Phase)stateMachine.CurrentState));
            }
        }

        private Phase GetNextPhase(PhaseType phaseType)
        {
            if(isFirstRound)
            {
                isFirstRound = false;
                return typeToPhase[OrderData.Order[0]];
            }
            var nextState = phaseType.Next(OrderData);
            return (typeToPhase[nextState]);
        }

        private Phase GetNextPhase(Phase phase)
        {
            return GetNextPhase(phase.Type);
        }

        private void OnStateChanged(State previousState, State currentState)
        {
            if (previousState != null)   
                PhaseExited?.Invoke(((Phase)previousState).Type);
            if (currentState == null)
            {
                Debug.LogError("Current State can't be null!");
                return;
            }
            PhaseEntered?.Invoke(((Phase)currentState).Type);
        }
    }
}

