using System.Linq;
using LineWars.Controllers;

namespace LineWars
{
    public partial class PhaseManager
    {
        // одновременно
        public class PhaseSynchronousState : Phase
        {
            private int actorsLeft;
            private bool AreActorsDone => actorsLeft == 0;

            public PhaseSynchronousState(PhaseType phase, PhaseManager phaseManager) : base(phase, phaseManager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                var turnLogics = manager.Actors
                    .Where((actor) => actor.CanExecuteTurn(Type))
                    .Select((actor) => actor.GetTurnLogic(Type))
                    .ToArray();
                actorsLeft = turnLogics.Length;
                foreach (var logic in turnLogics) 
                    logic.Ended += OnTurnLogicEnd;
                foreach(var logic in turnLogics)
                    logic.Start();
            }

            private void OnTurnLogicEnd(ITurnLogic logic)
            {
                logic.Ended -= OnTurnLogicEnd;
                actorsLeft--;
                
                if(AreActorsDone)
                    manager.ToNextPhase();
            }
        }
    }
}
