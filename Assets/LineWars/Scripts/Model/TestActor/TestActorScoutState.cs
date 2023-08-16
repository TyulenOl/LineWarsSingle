using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class TestActorScoutState : State
    {
        private readonly TestActor actor;
        private readonly Action<PhaseType> setNewPhase;
        public TestActorScoutState(TestActor actor, Action<PhaseType> setNewPhase)
        {
            this.actor = actor;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            Debug.Log($"{actor}: starting SCOUT TURN");
            setNewPhase(PhaseType.Scout);
            actor.StartCoroutine(TimerCoroutine());
        }

        public override void OnExit()
        {
            Debug.Log($"{actor}: ending SCOUT TURN");
            setNewPhase(PhaseType.Idle);
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(actor.MinOtherTime, actor.MaxOtherTime));
            actor.ScoutLeft--;
            Debug.Log($"{actor} scouted!");
            actor.ExecuteTurn(PhaseType.Idle);
        }
    }
}

