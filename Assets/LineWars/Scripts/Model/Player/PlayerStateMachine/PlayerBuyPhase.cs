using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class PlayerBuyPhase : PlayerPhase
    {
        public PlayerBuyPhase(Player player, PhaseType phase,
                                 Action<PhaseType> setExecutors, Action<Unit, bool> setUnitUsed) 
        : base(player, phase, setExecutors, setUnitUsed)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("PLAYER do be shopping *devil emoji*");
            player.StartCoroutine(IdleCroroutine());
            IEnumerator IdleCroroutine()
            {
                yield return null;
                player.ExecuteTurn(PhaseType.Idle);
            }
        }
    }
}
