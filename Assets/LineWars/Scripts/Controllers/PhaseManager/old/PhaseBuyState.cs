using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class PhaseBuyState : State
    {
        private readonly PhaseManager manager;
        private readonly Action<PhaseType> setNewPhase;
        private int actorsLeft;

        private readonly Dictionary<IActor, bool> actorsReadiness;
        public bool AreActorsDone => actorsLeft <= 0;
        public PhaseBuyState(PhaseManager manager, Action<PhaseType> changePhaseMethod)
        {
            this.manager = manager;
            setNewPhase = changePhaseMethod;
            actorsReadiness = new Dictionary<IActor, bool>();
        }
        
        public override void OnEnter()
        {
            setNewPhase(PhaseType.Buy);
            actorsLeft = manager.Actors.Count;
            foreach(var actor in manager.Actors)
            {
                actorsReadiness[actor] = false;
                actor.ExecuteTurn(PhaseType.Buy);
            }

            manager.ActorTurnEnded += OnActorTurnEnded;
        }

        public override void OnExit()
        {
            setNewPhase(PhaseType.Idle);
            actorsLeft = manager.Actors.Count;

            manager.ActorTurnEnded -= OnActorTurnEnded;
        }

        private void OnActorTurnEnded(IActor actor, PhaseType phase)
        {
            if(actorsReadiness[actor])
            {
                Debug.LogWarning($"{actor} has already end his turn!");
                return;
            }
            actorsLeft--;
            actorsReadiness[actor] = true;
        }
    }
}
