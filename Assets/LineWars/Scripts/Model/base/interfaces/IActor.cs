using System;

namespace LineWars.Model
{
    public interface IActor
    {
        public event Action<PhaseType, PhaseType> TurnChanged;
        public PhaseType CurrentPhase {get;}
        public bool CanExecuteTurn(PhaseType phaseType);
        public void ExecuteTurn(PhaseType phaseType);

        public void FinishTurn() => ExecuteTurn(PhaseType.Idle);
    }
}

