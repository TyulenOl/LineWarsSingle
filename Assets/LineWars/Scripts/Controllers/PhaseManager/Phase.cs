using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model 
{
    public class Phase : State
    {
        private readonly PhaseType _type;
        public PhaseType Type => _type;
        protected readonly PhaseManager manager;
        public virtual bool AreActorsDone => manager.Actors.All((actor) => !actor.CanExecuteTurn(Type));

        public Phase(PhaseType phase, PhaseManager phaseManager)
        {
            _type = phase;
            manager = phaseManager;
        }
    }
}

