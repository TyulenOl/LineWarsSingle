using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public partial class PhaseManager
    {
        // оновременно
        public class PhaseSimultaneousState : Phase
        {
            private int actorsLeft;
            private readonly Dictionary<IActor, bool> actorsReadiness;
            public override bool AreActorsDone => actorsLeft <= 0;

            public PhaseSimultaneousState(PhaseType phase, PhaseManager phaseManager) : base(phase, phaseManager)
            {
                actorsReadiness = new Dictionary<IActor, bool>();
                actorsLeft = manager.Actors.Count;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                actorsLeft = 0;
                manager.ActorTurnChanged += OnActorsTurnChanged;
                foreach(var actor in manager.Actors)
                {
                    actorsReadiness[actor] = true;
                    if(actor.CanExecuteTurn(Type))
                    {
                        actorsReadiness[actor] = false;
                        actorsLeft++;
                        actor.ExecuteTurn(Type);
                    }
                    
                }
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
                        Debug.LogWarning($"{actor} ended turn {previousPhase};");
                    return;
                }
                if(actorsReadiness[actor])
                    Debug.LogError($"{actor} ended turn {previousPhase}; He ended {Type} earlier!");

                actorsLeft--;
                actorsReadiness[actor] = true;
            }
        }
    }
}
