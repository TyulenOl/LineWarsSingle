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
        public bool CanExecuteTurn(PhaseType phaseType)
        {
            switch(phaseType)
            {
                case PhaseType.Idle:
                    return true;
                case PhaseType.Buy:
                    return CanExecuteBuy();
                case PhaseType.Artillery:
                    return CanExecuteArtillery();
                case PhaseType.Fight:
                    return CanExecuteFight();
                case PhaseType.Scout:
                    return CanExecuteScout();
            }
            Debug.LogWarning
            ($"Phase.{phaseType} is not implemented in \"CanExecuteTurn\"! "
            + "Change IActor to acommodate for this phase!");
            return false;
        }
        public void ExecuteTurn(PhaseType phaseType)
        {
            switch(phaseType)
            {
                case PhaseType.Buy:
                    ExecuteBuy();
                    break;
                case PhaseType.Artillery:
                    ExecuteArtillery();
                    break;
                case PhaseType.Fight:
                    ExecuteFight();
                    break;
                case PhaseType.Scout:
                    ExecuteScout();
                    break;
                default:
                    Debug.LogWarning($"Phase.{phaseType} is not implemented in \"ExecuteTurn\"! "
                    + "Change IActor to acommodate for this phase!");
                    break;
            }
        }
        public void EndTurn();

        #region Turns
        public void ExecuteBuy();
        public void ExecuteArtillery();
        public void ExecuteFight();
        public void ExecuteScout();
        #endregion

        #region Check Turns
        public bool CanExecuteBuy();
        public bool CanExecuteArtillery();
        public bool CanExecuteFight();
        public bool CanExecuteScout();
        #endregion

    }
}

