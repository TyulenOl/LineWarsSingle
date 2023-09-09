using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;

namespace LineWars
{
    public partial class PhaseManager
    {
        public class PhaseAlternatingState : Phase
        {
            private IActor currentActor => manager.Actors[manager.currentActorId];

            public override bool AreActorsDone
            {
                get
                {
                    var actorsAreFree = true;
                    foreach (var actor in manager.actors)
                    {
                        if (actor.CurrentPhase == Type)
                        {
                            actorsAreFree = false;
                            break;
                        }
                    }

                    return base.AreActorsDone && actorsAreFree;
                }
            }

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
            
            private void Begin()
            {
                if(AreActorsDone) return;
                
                PickNewActor();
                currentActor.ExecuteTurn(Type);
            }
            
            private bool PickNewActor()
            {
                if(AreActorsDone) return false;

                var potentialActorId = manager.currentActorId;
                while(true)
                {
                    potentialActorId = (potentialActorId + 1) % manager.Actors.Count;
                    if(manager.Actors[potentialActorId].CanExecuteTurn(Type))
                    {
                        manager.currentActorId = potentialActorId;
                        return true;
                    }
                }
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

                if (actor != currentActor)
                {
                    Debug.LogError($"{currentActor} is making a turn; {actor} is ended the turn instead!");
                    return;
                }

                if (PickNewActor())
                    currentActor.ExecuteTurn(Type);
            }
        }
    }
}

