using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;
using LineWars.Controllers;

namespace LineWars
{
    public partial class PhaseManager
    {
        public class PhaseAlternatingState : Phase
        {
            private ITurnLogic currentTurnLogic;

            public PhaseAlternatingState(PhaseType phase, PhaseManager manager) : base(phase, manager)
            {
                
            }

            public override void OnEnter()
            {
                base.OnEnter();
                currentTurnLogic = null;
            }

            private void CycleNewActor()
            {
                var nextActorId = FindNextActor(manager.Actors, manager.currentActorId);
                if (nextActorId == -1)
                {
                    manager.ToNextPhase();
                    return;
                }
                manager.currentActorId = nextActorId;
                currentTurnLogic = manager.CurrentActor.GetTurnLogic(Type);
                currentTurnLogic.Ended += OnTurnLogicEnd;
                currentTurnLogic.Start();
            }

            private void OnTurnLogicEnd(ITurnLogic _)
            {
                currentTurnLogic.Ended -= OnTurnLogicEnd;
                currentTurnLogic = null;
                manager.StartCoroutine(CycleCoroutine());
                
                IEnumerator CycleCoroutine()
                { 
                    yield return null;
                    CycleNewActor();
                }
            }

            private int FindNextActor(IReadOnlyList<IActor> actors, int currentActorId)
            {
                var potentialActorId = currentActorId;
                var startActorId = currentActorId;
                while (true)
                {
                    potentialActorId = (potentialActorId + 1) % actors.Count;    
                    if (actors[potentialActorId].CanExecuteTurn(Type))
                        return potentialActorId;
                    if (startActorId == potentialActorId)
                        return -1;
                }
            }
        }
    }
}

