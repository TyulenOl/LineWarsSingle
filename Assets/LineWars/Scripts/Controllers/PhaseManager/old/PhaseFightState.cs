using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class PhaseFightState : State
    {
        private readonly PhaseManager manager;
        private readonly Action<PhaseType> setNewPhase;
        
        private int actorId;
        public bool AreActorsDone => !manager.Actors.Any((actor) => actor.CanExecuteTurn(PhaseType.Fight));

        public PhaseFightState(PhaseManager manager, Action<PhaseType> setNewPhase)
        {
            this.manager = manager;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            setNewPhase(PhaseType.Fight);
            actorId = -1;
            StartNewActorsTurn(manager.CurrentPhase);
        }

        public override void OnExit()
        {
            setNewPhase(PhaseType.Idle);
        }

        private void StartNewActorsTurn(PhaseType phaseType)
        {
            Debug.Log("Trying to start a new turn");
            if(AreActorsDone) 
            {
                Debug.Log("Can't do shit!");
                return;
            }
            if(phaseType != manager.CurrentPhase) 
                Debug.LogError($"{manager.Actors[actorId]} ended Phase {phaseType}; PhaseManager is in phase {manager.CurrentPhase}");
            if(actorId != -1)
                manager.Actors[actorId].TurnEnded -= StartNewActorsTurn;
                
            while(true)
            {
                actorId = (actorId + 1) % manager.Actors.Count;
                if(manager.Actors[actorId].CanExecuteTurn(manager.CurrentPhase))
                {
                    manager.Actors[actorId].TurnEnded += StartNewActorsTurn;
                    manager.Actors[actorId].StartTurn(manager.CurrentPhase);
                    break;
                }
            }
        }   
    }
}

