using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public partial class Player
    {
        public class PlayerReplenishPhase : PlayerPhase
        {
            public PlayerReplenishPhase(Player player, PhaseType phase) : base(player, phase)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();

                foreach(var unit in player.UnitUsage.Keys.ToArray())
                {
                    unit.OnTurnEnd();
                    player.unitUsage[unit] = false;
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
}

