using System;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class ProgrammedAI : BasePlayer
    {
        [SerializeField] private SerializedDictionary<PhaseType, AITurns> turns;

        protected override bool CanExecuteReplenish() => true;

        protected override bool CanExecuteArtillery()
        {
            return CanExecuteTurnProgrammedAITurn(PhaseType.Artillery);
        }

        protected override bool CanExecuteScout()
        {
            return CanExecuteTurnProgrammedAITurn(PhaseType.Scout);
        }

        protected override bool CanExecuteFight()
        {
            return CanExecuteTurnProgrammedAITurn(PhaseType.Fight);
        }

        protected override bool CanExecuteBuy()
        {
            return CanExecuteTurnProgrammedAITurn(PhaseType.Buy);
        }

        private bool CanExecuteTurnProgrammedAITurn(PhaseType phaseType)
        {
            return turns.TryGetValue(phaseType, out var phaseTurns);
        }

        protected override void ExecuteFight()
        {
            base.ExecuteFight();
            turns[PhaseType.Fight].GetNextTurn().Execute();
            FinishTurn();
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            FinishTurn();
        }
    }
}