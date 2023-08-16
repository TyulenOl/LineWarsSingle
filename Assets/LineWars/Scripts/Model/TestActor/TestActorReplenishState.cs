using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class TestActorReplenishState : State
    {
        private readonly TestActor actor;
        private readonly Action<PhaseType> setNewPhase;
        public TestActorReplenishState(TestActor actor, Action<PhaseType> setNewPhase)
        {
            this.actor = actor;
            this.setNewPhase = setNewPhase;
        }

        public override void OnEnter()
        {
            Debug.Log($"{actor} starting REPLENISH TURN");
            setNewPhase(PhaseType.Replenish);
            actor.ArtilleryLeft = actor.ArtilleryUnits;
            actor.FightLeft = actor.FightUnits;
            actor.ScoutLeft = actor.ScoutUnits;
            
            actor.StartCoroutine(ReplenishCoroutine());
        }

        public override void OnExit()
        {
            Debug.Log($"{actor} ending REPLENISH TURN");
            setNewPhase(PhaseType.Idle);
        }

        private IEnumerator ReplenishCoroutine()
        {
            yield return null;
            actor.ExecuteTurn(PhaseType.Idle);
        }


    }
}

