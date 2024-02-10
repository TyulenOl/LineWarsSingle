using UnityEngine;
using System.Linq;
using LineWars.Model;

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
                var elligbleActors = manager.Actors
                    .Where((actor) => actor.CanExecuteTurn(Type))
                    .ToArray();
                actorsLeft = elligbleActors.Length;
                if(AreActorsDone )
                {
                    manager.ToNextPhase();
                    return;
                }
                foreach (var actor in elligbleActors) 
                    actor.TurnEnded += OnTurnLogicEnd;
                foreach(var actor in elligbleActors)
                    actor.ExecuteTurn(Type);
            }

            private void OnTurnLogicEnd(IActor actor, PhaseType phaseType)
            {
                if (phaseType != Type)
                    Debug.LogWarning($"IActor Ended Wrong Phase! was: {phaseType}; should be: {Type}");
                actor.TurnEnded -= OnTurnLogicEnd;
                actorsLeft--;
                
                if(AreActorsDone)
                    manager.ToNextPhase();
            }
        }
    }
}
