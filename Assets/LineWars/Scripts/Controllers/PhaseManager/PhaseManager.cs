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
        
        public event Action<IActor, PhaseType, PhaseType> ActorTurnChanged;

        public UnityEvent<PhaseType, PhaseType> PhaseChanged;

        private StateMachine stateMachine;
        private Phase idleState;
        private PhaseSimultaneousState buyState;
        private PhaseAlternatingState artilleryState;
        private PhaseAlternatingState fightState;
        private PhaseAlternatingState scoutState;
        private PhaseSimultaneousState replenishState;
        
        private Dictionary<PhaseType, Phase> typeToPhase;
        private int currentActorId;

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
            IntitializeStateMachine();
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

        private void OnDisable() 
        {
            foreach(var actor in actors)
            {
                actor.TurnChanged -= GetInvokingEndingTurn(actor);
            }
            stateMachine.SetState(idleState);
        }
        
        public void StartGame()
        {
            Debug.Log("Game Started!");
            stateMachine.SetState(typeToPhase[OrderData.Order[0]]);
        }

        public void RegisterActor(IActor actor)
        {
            if(actors.Contains(actor))
            {
                Debug.LogError($"{actor} is already registered!");
            }
            actors.Add(actor);
            actor.TurnChanged += GetInvokingEndingTurn(actor);
        }

        public void ForceSkipPhase()
        {
            switch (CurrentPhase)
            {
                case PhaseType.Buy:
                    stateMachine.SetState(artilleryState);
                    break;
                case PhaseType.Artillery:
                    stateMachine.SetState(fightState);
                    break;
                case PhaseType.Fight:
                    stateMachine.SetState(scoutState);
                    break;
                case PhaseType.Scout:
                    stateMachine.SetState(replenishState);
                    break;
            }
        }

        private void IntitializeStateMachine()
        {
            stateMachine = new StateMachine();
            idleState = new Phase(PhaseType.Idle, this);
            buyState = new PhaseSimultaneousState(PhaseType.Buy, this);
            artilleryState = new PhaseAlternatingState(PhaseType.Artillery, this);
            fightState = new PhaseAlternatingState(PhaseType.Fight, this);
            scoutState = new PhaseAlternatingState(PhaseType.Scout, this);
            replenishState = new PhaseSimultaneousState(PhaseType.Replenish, this);
            
            typeToPhase = new Dictionary<PhaseType, Phase>()
            {
                {PhaseType.Buy, buyState},
                {PhaseType.Artillery, artilleryState},
                {PhaseType.Fight, fightState},
                {PhaseType.Scout, scoutState},
                {PhaseType.Replenish, replenishState}
            };

            InitializeTransitions();
        }

        private void InitializeTransitions()
        {
            for(var i = 0; i < OrderData.Order.Count - 1; i++)
            {
                var from = typeToPhase[OrderData.Order[i]];
                var to = typeToPhase[OrderData.Order[i + 1]];
                stateMachine.AddTransition(from, to, () => from.AreActorsDone);
            }

            var firstState = typeToPhase[OrderData.Order[0]];
            var lastState = typeToPhase[OrderData.Order[OrderData.Order.Count - 1]];
            stateMachine.AddTransition(lastState, firstState, () => lastState.AreActorsDone);
        }

        private void OnStateChanged(State previousState, State currentState)
        {
            PhaseType previousPhase = PhaseType.Idle;
            if(previousState != null)
                previousPhase = ((Phase)previousState).Type;
            
            PhaseChanged.Invoke(previousPhase, ((Phase)currentState).Type);
        }

        private Action<PhaseType, PhaseType> GetInvokingEndingTurn(IActor actor)
        {
            return (previousType, currentType) => ActorTurnChanged?.Invoke(actor, previousType, currentType);
        }
    }
}

