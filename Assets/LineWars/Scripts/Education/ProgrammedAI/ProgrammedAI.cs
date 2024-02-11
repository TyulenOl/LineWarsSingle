using UnityEngine;

namespace LineWars.Model
{
    public class ProgrammedAI : BasePlayer
    {
        [SerializeField] public ProgrammedAIActions turns;
        public override bool CanExecuteTurn(PhaseType phaseType)
        {
            if (!base.CanExecuteTurn(phaseType))
                return false;
            
            if (phaseType == PhaseType.Buy || 
                phaseType == PhaseType.Replenish ||
                phaseType == PhaseType.Payday)
                return true;
            
            foreach (var owned in OwnedObjects)
            {
                if (owned is not Unit unit) continue;
                if (unit.CurrentActionPoints > 0)
                    return turns.ContainsNext();
            }

            return false;
        }

        public override void ExecuteTurn(PhaseType phaseType)
        {
            InvokeTurnStarted(phaseType);
            if (phaseType == PhaseType.Replenish)
            {
                ExecuteReplenish();
                InvokeTurnEnded(phaseType);
                return;
            }

            if (phaseType == PhaseType.Payday)
            {
                ExecutePayday();
                InvokeTurnEnded(phaseType);
            }
            
            if (turns.TryGetNext(out var turn))
            {
                if (turn is IAIActionWithNeedProgrammedPlayer needProgrammedPlayer)
                    needProgrammedPlayer.Prepare(this);
                turn.Execute();
            }
            
            InvokeTurnEnded(phaseType);
        }
    }
}