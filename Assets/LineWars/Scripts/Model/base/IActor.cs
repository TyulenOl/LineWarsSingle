using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LineWars.Controllers;

namespace LineWars.Model
{
    public interface IActor
    {
        public event Action<PhaseType, PhaseType> TurnChanged;
        public PhaseType CurrentPhase {get;}
        public bool CanExecuteTurn(PhaseType phaseType);
        public void ExecuteTurn(PhaseType phaseType);
    }
}

