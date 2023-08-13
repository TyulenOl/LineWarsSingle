using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{  
    public enum PhaseType
    {
        Idle,
        Buy,
        Artillery,
        Fight,
        Scout
    }

    public class PhaseManager : MonoBehaviour
    {
        private List<IActor> actors;

        public event Action<IActor, PhaseType> ActorTurnEnded;
        public event Action<PhaseType, PhaseType> PhaseChanged;

        private StateMachine stateMachine;
        private Phase idleState;
        private PhaseSimultaneousState buyState;
        private PhaseAlternatingState artilleryState;
        private PhaseAlternatingState fightState;
        private PhaseAlternatingState scoutState;
        


        public IReadOnlyList<IActor> Actors => actors.AsReadOnly();
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
        }
        private void Start() 
        {
            Invoke("StartGame", 3f);
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
                actor.TurnEnded -= GetInvokingEndingTurn(actor);
            }
            stateMachine.SetState(idleState);
        }

        private void IntitializeStateMachine()
        {
            stateMachine = new StateMachine();
            idleState = new Phase(PhaseType.Idle, this);
            buyState = new PhaseSimultaneousState(PhaseType.Buy, this);
            artilleryState = new PhaseAlternatingState(PhaseType.Artillery, this);
            fightState = new PhaseAlternatingState(PhaseType.Fight, this);
            scoutState = new PhaseAlternatingState(PhaseType.Scout, this);

            stateMachine.AddTransition(buyState, artilleryState, () => buyState.AreActorsDone);
            stateMachine.AddTransition(artilleryState, fightState, () => artilleryState.AreActorsDone);
            stateMachine.AddTransition(fightState, scoutState, () => fightState.AreActorsDone);
            stateMachine.AddTransition(scoutState, buyState, () => scoutState.AreActorsDone);
        }

        public void StartGame()
        {
            Debug.Log("Game Started!");
            stateMachine.SetState(buyState);
        }

        public void RegisterActor(IActor actor)
        {
            if(actors.Contains(actor))
            {
                Debug.LogError($"{actor} is already registered!");
            }
            actors.Add(actor);
            actor.TurnEnded += GetInvokingEndingTurn(actor);
        }

        private Action<PhaseType> GetInvokingEndingTurn(IActor actor)
        {
            return (phaseType) => ActorTurnEnded?.Invoke(actor, phaseType);
        }
    }
}

