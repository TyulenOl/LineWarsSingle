using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public partial class PhaseManager
    {
        public class Phase : State
        {
            private readonly PhaseType type;
            public PhaseType Type => type;
            protected readonly PhaseManager manager;
            public virtual bool AreActorsDone => manager.Actors.All((actor) => !actor.CanExecuteTurn(Type));

            public Phase(PhaseType phase, PhaseManager phaseManager)
            {
                type = phase;
                manager = phaseManager;
            }

            public override void OnEnter()
            {
                Debug.Log($"STARTED {Type}");
            }

            public override void OnExit()
            {
                Debug.Log($"ENDED {Type}");
            }
        }
    }
    
}

