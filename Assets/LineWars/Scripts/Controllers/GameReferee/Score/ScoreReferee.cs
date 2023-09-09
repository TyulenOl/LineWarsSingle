using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public abstract class ScoreReferee : GameReferee
    {
        [SerializeField, Min(0)] private int scoreForWin;
        
        private Dictionary<BasePlayer, int> playersScore;

        public event Action<BasePlayer, int, int> ScoreChanged;
        public int ScoreForWin => scoreForWin;

        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);
            playersScore = new Dictionary<BasePlayer, int>
            (enemies
                .Concat(new[] {me})
                .Select(x => new KeyValuePair<BasePlayer, int>(x, 0))
            );

            foreach (var (player, score) in playersScore)
                player.Defeated += () =>
                {
                    playersScore.Remove(player);
                    if (player == me)
                        Lose();
                    if (playersScore.Count == 1) // остался только игрок
                        Win();
                };
        }

        public int GetScoreForPlayer(BasePlayer basePlayer)
        {
            return playersScore.TryGetValue(basePlayer, out var score) ? score : 0;
        }

        protected void SetScoreForPlayer([NotNull] BasePlayer basePlayer, int score)
        {
            if (basePlayer == null) throw new ArgumentNullException(nameof(basePlayer));
            
            var before = playersScore[basePlayer];
            playersScore[basePlayer] = score;
            ScoreChanged?.Invoke(basePlayer, before, score);

            if (GetScoreForPlayer(basePlayer) >= scoreForWin)
            {
                if (basePlayer == Me)
                    Win();
                else
                    Lose();
            }
        }
    }
}