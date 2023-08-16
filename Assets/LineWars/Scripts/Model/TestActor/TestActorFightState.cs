using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class TestActorFightState : State
    {
        private readonly TestActor actor;
        private readonly Action<PhaseType> setNewPhase;
        public TestActorFightState(TestActor actor, Action<PhaseType> setNewPhase)
        {
            this.actor = actor;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            Debug.Log($"{actor}: starting FIGHT TURN");
            setNewPhase(PhaseType.Fight);
            actor.StartCoroutine(TimerCoroutine());
        }

        public override void OnExit()
        {
            Debug.Log($"{actor}: ending FIGHT TURN");
            setNewPhase(PhaseType.Idle);
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(actor.MinOtherTime, actor.MaxOtherTime));
            actor.FightLeft--;
            Debug.Log($"{actor} fought!");
            actor.ExecuteTurn(PhaseType.Idle);
        }
    
    }
}

