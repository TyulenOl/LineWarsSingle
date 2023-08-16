using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class TestActorArtilleryState : State
    {
        private readonly TestActor actor;
        private readonly Action<PhaseType> setNewPhase;
        public TestActorArtilleryState(TestActor actor, Action<PhaseType> setNewPhase)
        {
            this.actor = actor;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            Debug.Log($"{actor}: starting ARTILLERY TURN");
            setNewPhase(PhaseType.Artillery);
            actor.StartCoroutine(TimerCoroutine());
        }

        public override void OnExit()
        {
            Debug.Log($"{actor}: ending ARTILLERY TURN");
            setNewPhase(PhaseType.Idle);
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(actor.MinOtherTime, actor.MaxOtherTime));
            actor.ArtilleryLeft--;
            Debug.Log($"{actor} artilled!");
            actor.ExecuteTurn(PhaseType.Idle);
        }
    }
}
