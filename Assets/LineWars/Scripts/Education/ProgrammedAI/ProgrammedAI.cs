using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    public class ProgrammedAI : BasePlayer
    {
        [SerializeField] private SerializedDictionary<PhaseType, AIActions> turns;

        protected override bool CanExecuteReplenish() => true;

        protected override bool CanExecuteArtillery()
        {
            return CanExecuteProgrammedAITurn(PhaseType.Artillery);
        }

        protected override bool CanExecuteScout()
        {
            return CanExecuteProgrammedAITurn(PhaseType.Scout);
        }

        protected override bool CanExecuteFight()
        {
            return CanExecuteProgrammedAITurn(PhaseType.Fight);
        }

        protected override bool CanExecuteBuy()
        {
            return CanExecuteProgrammedAITurn(PhaseType.Buy);
        }

        private bool CanExecuteProgrammedAITurn(PhaseType phaseType)
        {
            return turns.TryGetValue(phaseType, out var phaseTurns) && phaseTurns.ContainsNext();
        }
        
        protected override void ExecuteScout()
        {
            ExecuteProgrammedAITurn(PhaseType.Scout);
        }
        
        protected override void ExecuteArtillery()
        {
            ExecuteProgrammedAITurn(PhaseType.Artillery);
        }

        protected override void ExecuteFight()
        {
            ExecuteProgrammedAITurn(PhaseType.Fight);
        }

        protected override void ExecuteBuy()
        {
            FinishTurn();
        }

        private void ExecuteProgrammedAITurn(PhaseType phaseType)
        {
            base.ExecuteFight();
            if (turns[phaseType].TryGetNext(out var turn))
            {
                turn.Execute();
            }
            else
            {
                Debug.LogError($"Программируемый ИИ не может продолжить ход!");
            }
            FinishTurn();
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            FinishTurn();
        }
    }
}