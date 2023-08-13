using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LineWars.Model
{
    public interface IActor
    {
        public event Action<PhaseType> TurnStarted;
        public event Action<PhaseType> TurnEnded;
        public PhaseType CurrentPhase {get;}
        public bool IsInTurn {get;}
        public bool CanExecuteTurn(PhaseType phaseType);
        public void StartTurn(PhaseType phaseType);
        public void EndTurn();
        

    }
}

