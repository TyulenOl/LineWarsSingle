using System;
using JetBrains.Annotations;
using LineWars.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    public class NewDominationGameReferee : GameReferee
    {
        [SerializeField] private int rounds;

        private int pastRounds = -1;
        
        private int PastRounds
        {
            get => pastRounds;
            set
            {
                pastRounds = value;
                RoundsAmountChanged?.Invoke();
            }
        }
        
        public int RoundsToWin => rounds - PastRounds;

        public event Action RoundsAmountChanged;
        
        public override void Initialize([NotNull] Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
        }

        private void OnPhaseChanged(PhaseType previousType, PhaseType currentType)
        {
            if (currentType != PhaseType.Replenish)
                return;
            PastRounds++;
            if (PastRounds >= rounds)
            {
                CalculateVictory();
                PhaseManager.Instance.PhaseChanged.RemoveListener(OnPhaseChanged);
            }
        }

        private void CalculateVictory()
        {
            var winner = SingleGame.Instance.AllPlayers.Values
                .Select(player => (player.MyNodes.Count(), player))
                .MaxItem((playerPair1, playerPair2) => playerPair1.Item1.CompareTo(playerPair2.Item1))
                .Item2;
            if (winner == Me)
                Win();
            else
                Lose();
        }
    }
}
