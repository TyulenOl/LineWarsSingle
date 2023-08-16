using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class PlayerReplenishPhase : PlayerPhase
    {
        public PlayerReplenishPhase(Player player, PhaseType phase,
                                 Action<PhaseType> setExecutors, Action<Unit, bool> setUnitUsed) 
        : base(player, phase, setExecutors, setUnitUsed)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
    
            foreach(var unitData in player.IsUnitUsed)
            {
                unitData.Key.OnTurnEnd();
                setUnitUsed(unitData.Key, false);
            }

            player.StartCoroutine(IdleCroroutine());
            IEnumerator IdleCroroutine()
            {
                yield return null;
                player.ExecuteTurn(PhaseType.Idle);
            }
        }
    }
}

