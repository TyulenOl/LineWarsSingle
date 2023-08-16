using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class TestActorBuyState : State
    {
        private readonly TestActor actor;
        private Action<PhaseType> setNewPhase;
        public TestActorBuyState(TestActor actor, Action<PhaseType> setNewPhase)
        {
            this.actor = actor;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            Debug.Log($"{actor}: starting BUY TURN!!");
            setNewPhase(PhaseType.Buy);
            actor.StartCoroutine(TimerCoroutine());

            actor.ArtilleryLeft = actor.ArtilleryUnits;
            actor.FightLeft = actor.FightUnits;
            actor.ScoutLeft = actor.ScoutUnits;
        }

        public override void OnExit()
        {
            Debug.Log($"{actor}: ending BUY TURN!!!");
            setNewPhase(PhaseType.Idle);
        }

        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(actor.MinBuyTime, actor.MaxBuyTime));
            Debug.Log($"{actor} bought!");
            actor.ExecuteTurn(PhaseType.Idle);
        }
    }
}

