using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class WallToWallGameReferee : GameReferee
    {
        protected override bool noBaseWinCondition => false;
        
        public event Action<BasePlayer, int> ScoreChanged;
        
        public override void Initialize(Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
            //player.PhaseExceptions.Add(PhaseType.Buy);
            foreach(var enemy in enemies)
            {
                //enemy.PhaseExceptions.Add(PhaseType.Buy);
                enemy.OwnedRemoved += Enemy_OwnerRemoved;
                enemy.PlayerOwnedAdded += OnPlayerOwnedAdded;
            }
            player.OwnedRemoved += Me_OwnedRemoved;
            player.PlayerOwnedAdded += OnPlayerOwnedAdded;
        }



        public int GetScoreForPlayer()
        {
            return Player.MyUnits.Count();
        }
        
        public int GetScoreForEnemies()
        {
            return Enemies.First().MyUnits.Count();
        }

        private void Me_OwnedRemoved(Owned obj)
        {
            ScoreChanged?.Invoke(Player, Player.MyUnits.Count());
            
            if (!Player.MyUnits.Any())
                Lose();
        }

        private void Enemy_OwnerRemoved(Owned obj)
        {
            foreach(var enemy in Enemies)
            {
                ScoreChanged?.Invoke(enemy, enemy.MyUnits.Count());
                if (enemy.MyUnits.Count() != 0)
                    return;
            }
            
            Win();
        }
        
        private void OnPlayerOwnedAdded(BasePlayer player, Owned obj)
        {
            ScoreChanged?.Invoke(player, player.MyUnits.Count());
        }
    }
}
