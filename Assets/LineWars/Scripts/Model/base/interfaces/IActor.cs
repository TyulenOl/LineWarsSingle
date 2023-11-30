using LineWars.Controllers;
using System;

namespace LineWars.Model
{
    public interface IActor
    {
        public event Action<IActor> TurnStarted;
        public event Action<IActor> TurnEnded;
        public bool CanExecuteTurn(PhaseType phaseType);
        public void ExecuteTurn(PhaseType phaseType);
    }
}

