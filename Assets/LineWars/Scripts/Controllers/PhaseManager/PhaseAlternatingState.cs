using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Controllers
{
    public partial class PhaseManager
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
                base.OnEnter();
                if(AreActorsDone) return;
                manager.ActorTurnChanged += OnActorsTurnChanged;
                Begin();
            }

            public override void OnExit()
            {
                base.OnExit();
                manager.ActorTurnChanged -= OnActorsTurnChanged;
            }

            private void OnActorsTurnChanged(IActor actor, PhaseType previousPhase, PhaseType currentPhase)
            {
                if(previousPhase != Type)
                {
                    if(previousPhase != PhaseType.Idle)
                    {
                        Debug.LogError($"{actor} ended turn {previousPhase}; Phase {Type} is parsing it instead!");
                    }
                    return;
                }
                if(actor != currentActor)
                    Debug.LogError($"{currentActor} is making a turn; {actor} is ended the turn instead!");

                if(PickNewActor())
                    currentActor.ExecuteTurn(Type);

            }

            private void Begin()
            {
                if(AreActorsDone) return;
                currentActorId = 0;
                if(!currentActor.CanExecuteTurn(Type))
                {
                    PickNewActor();
                }

                currentActor.ExecuteTurn(Type);
                
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
}

