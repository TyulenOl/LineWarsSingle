using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class PlayerPhase : State
    {
        protected readonly Player player;
        protected readonly PhaseType phaseType;
        protected readonly Action<Unit, bool> setUnitUsed;
        private readonly Action<PhaseType> setExecutors;

        public PlayerPhase(Player player, PhaseType phase, Action<PhaseType> setExecutors, Action<Unit, bool> setUnitUsed)
        {
            this.player = player;
            phaseType = phase;
            this.setExecutors = setExecutors;
            this.setUnitUsed = setUnitUsed;
        }

        public override void OnEnter()
        {
            setExecutors(phaseType);
            CommandsManager.Instance.CommandExecuted.AddListener(OnCommandExecuted);
        }

        public override void OnExit()
        {
            CommandsManager.Instance.CommandExecuted.RemoveListener(OnCommandExecuted);
        }

        private void OnCommandExecuted(IExecutor executor, ITarget target)
        {
            if(executor is Unit unit)
            {
                setUnitUsed(unit, true);
            }
            player.ExecuteTurn(PhaseType.Idle);

        }
    }
}