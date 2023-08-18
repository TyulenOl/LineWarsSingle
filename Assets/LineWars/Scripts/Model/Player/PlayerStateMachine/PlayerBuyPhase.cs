using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public partial class Player 
    {
        public class PlayerBuyPhase : PlayerPhase
        {
            public PlayerBuyPhase(Player player, PhaseType phase) : base(player, phase)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Debug.Log("PLAYER do be shopping *devil emoji*");
                player.IsTurnMade = true;
            }
        }
    }
}
