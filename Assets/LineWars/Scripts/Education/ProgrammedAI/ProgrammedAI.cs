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

        private bool CanExecuteTurnProgrammedAITurn(PhaseType phaseType)
        {
            return turns.TryGetValue(phaseType, out var phaseTurns);
        }

        private void ExecuteEnemyProgrammedAITurn(PhaseType phaseType)
        {
        }
    }
}