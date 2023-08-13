using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class PhaseAlternatingState : Phase
    {
        private int currentActorId;
        private IActor currentActor => manager.Actors[currentActorId];
    
        public PhaseAlternatingState(PhaseType phase, PhaseManager manager) : base(phase, manager)
        {
            
        }

        public override void OnEnter()
        {
            if(AreActorsDone) return;
            manager.ActorTurnEnded += OnActorsTurnEnded;
            Begin();
        }

        public override void OnExit()
        {
            manager.ActorTurnEnded -= OnActorsTurnEnded;
        }

        private void OnActorsTurnEnded(IActor actor, PhaseType phase)
        {
            if(phase != Type)
                Debug.LogError($"{actor} ended turn {phase}; Phase {Type} is parsing it instead!");
            if(actor != currentActor)
                Debug.LogError($"{currentActor} is making a turn; {actor} is ended the turn instead!");

            if(PickNewActor())
                currentActor.StartTurn(Type);

        }

        private void Begin()
        {
            currentActorId = 0;
            currentActor.StartTurn(Type);
        }

        private bool PickNewActor()
        {
            if(AreActorsDone) return false;

            var potentialActorId = currentActorId;
            while(true)
            {
                potentialActorId = (potentialActorId + 1) % manager.Actors.Count;
                if(manager.Actors[potentialActorId].CanExecuteTurn(Type))
                {
                    currentActorId = potentialActorId;
                    return true;
                }
            }
        }
    }
}

