using LineWars.Controllers;
using System;

namespace LineWars.Model
{
    public interface IActor
    {
        public bool CanExecuteTurn(PhaseType phaseType);
        public ITurnLogic GetTurnLogic(PhaseType phaseType);
    }
}

