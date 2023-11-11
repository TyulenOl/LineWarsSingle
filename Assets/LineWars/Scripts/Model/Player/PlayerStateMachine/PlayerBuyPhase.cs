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
        }
    }
}
