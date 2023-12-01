using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public partial class PhaseManager
    {
        public class PhaseAlternatingState : Phase
        {
            public PhaseAlternatingState(PhaseType phase, PhaseManager manager) : base(phase, manager)
            { 
            }

            public override void OnEnter()
            {
                base.OnEnter();
                CycleNewActor();
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
                manager.CurrentActor.TurnEnded += OnTurnLogicEnd;
                manager.CurrentActor.ExecuteTurn(Type);
            }

            private void OnTurnLogicEnd(IActor _, PhaseType phaseType)
            {
                if (phaseType != Type)
                    Debug.LogWarning($"IActor Ended Wrong Phase! was: {phaseType}; should be: {Type}");
                manager.CurrentActor.TurnEnded -= OnTurnLogicEnd;
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