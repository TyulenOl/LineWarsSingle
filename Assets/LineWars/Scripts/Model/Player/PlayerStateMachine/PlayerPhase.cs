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
        public class PlayerPhase : State
        {
            protected readonly Player player;
            protected readonly PhaseType phaseType;

            public PlayerPhase(Player player, PhaseType phase)
            {
                this.player = player;
                phaseType = phase;
            }

            public override void OnEnter()
            {
                Debug.Log($"Player entered {phaseType}");
                player.potentialExecutors = player.PhaseExecutorsData.PhaseToUnits[phaseType];
                player.IsTurnMade = false;
            }

            public override void OnExit()
            {
                Debug.Log($"Player exited {phaseType}");
            }
        }
    }
}