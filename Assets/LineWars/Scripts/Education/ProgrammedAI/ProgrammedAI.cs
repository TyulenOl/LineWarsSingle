/*using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    public class ProgrammedAI : BasePlayer
    {
        [SerializeField] private SerializedDictionary<PhaseType, ProgrammedAIActions> turnsForPhase;

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
            return turnsForPhase.TryGetValue(phaseType, out var turns) && turns.ContainsNext();
        }


        protected override void ExecuteScout()
        {
            base.ExecuteScout();
            ExecuteProgrammedAITurn(PhaseType.Scout);
        }

        protected override void ExecuteArtillery()
        {
            base.ExecuteArtillery();
            ExecuteProgrammedAITurn(PhaseType.Artillery);
        }

        protected override void ExecuteFight()
        {
            base.ExecuteFight();
            ExecuteProgrammedAITurn(PhaseType.Fight);
        }

        protected override void ExecuteBuy()
        {
            base.ExecuteBuy();
            ExecuteProgrammedAITurn(PhaseType.Buy);
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            FinishTurn();
        }

        private void ExecuteProgrammedAITurn(PhaseType phaseType)
        {
            if (turnsForPhase[phaseType].TryGetNext(out var turn))
            {
                if (turn is IAIActionWithNeedProgrammedPlayer needProgrammedPlayer)
                    needProgrammedPlayer.Prepare(this);
                turn.Execute();
            }
            else
            {
                Debug.LogError($"Программируемый ИИ не может продолжить ход!");
            }

            FinishTurn();
        }
    }
}*/