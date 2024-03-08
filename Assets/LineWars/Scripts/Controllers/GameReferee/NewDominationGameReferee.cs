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
                RoundsAmountChanged?.Invoke(value, rounds);
            }
        }

        public int RoundsForWin => rounds;
        public int RoundsToWin => rounds - PastRounds;
        
        public event Action<int, int> RoundsAmountChanged;
        public event Action<BasePlayer, int> ScoreChanged;
        
        public override void Initialize([NotNull] Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
            PhaseManager.Instance.PhaseEntered.AddListener(OnPhaseEntered);

            foreach (var node in MonoGraph.Instance.Nodes)
            {
                node.OwnerChanged += NodeOnOwnerChanged;
            }
        }
        
        public int GetScoreForPlayer()
        {
            return Player.MyNodes.Count();
        }
        
        public int GetScoreForEnemies()
        {
            return Enemies.First().MyNodes.Count();
        }

        private void NodeOnOwnerChanged(BasePlayer before, BasePlayer after)
        {
            foreach (var basePlayer in AllPlayers)
                ScoreChanged?.Invoke(basePlayer, basePlayer.MyNodes.Count());
        }

        public void SetRounds(int value)
        {
            rounds = value;
        }

        private void OnPhaseEntered(PhaseType currentType)
        {
            if (currentType != PhaseType.Payday)
                return;
            PastRounds++;
            if (PastRounds >= rounds)
            {
                CalculateVictory();
                PhaseManager.Instance.PhaseEntered.RemoveListener(OnPhaseEntered);
            }
        }

        private void CalculateVictory()
        {
            var winner = SingleGameRoot.Instance.AllPlayers.Values
                .Select(player => (player.MyNodes.Count(), player))
                .MaxItem((playerPair1, playerPair2) => playerPair1.Item1.CompareTo(playerPair2.Item1))
                .Item2;
            if (winner == Player)
                Win();
            else
                Lose();
        }
    }
}
