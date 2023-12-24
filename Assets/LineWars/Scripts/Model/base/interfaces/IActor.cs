using LineWars.Controllers;
using System;

namespace LineWars.Model
{
    public interface IActor
    {
        public event Action<IActor, PhaseType> TurnStarted;
        public event Action<IActor, PhaseType> TurnEnded;
        public bool CanExecuteTurn(PhaseType phaseType);
        public void ExecuteTurn(PhaseType phaseType);
    }
}

